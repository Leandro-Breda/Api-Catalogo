using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTo = _mapper.Map<List<ProdutoDTO>>(produtos);

        return produtosDTo;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));    

            var produtosDTo = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDTo;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var produtoDTo = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTo;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]ProdutoDTO produtoDTo)
        {
            var produto = _mapper.Map<Produto>(produtoDTo);

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produtoDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDTo)
        {
            if(id != produtoDTo.ProdutoId)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDTo);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não localizado...");
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDTo = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTo;
        }
    }
}
