using APICatalogo.Context;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepo;
        private CategoriaRepository _categoriaRepo;
        public AppDbContext _uof;

        public UnitOfWork(AppDbContext context)
        {
            _uof = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_uof);
            }
        }


        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_uof);
            }
        }

        public async Task Commit()
        {
            await _uof.SaveChangesAsync();
        }
        public void Dispose()
        {
            _uof.Dispose();
        }
    }
}
