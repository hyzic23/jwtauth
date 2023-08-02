//using System;
//using Microsoft.EntityFrameworkCore;
//using jwtauth.dataaccess.IService;
//using jwtauth.dataaccess.Data;

//namespace jwtauth.dataaccess.Service
//{
//	public class GenericService : IGenericService<T> where T : class
//	{
//        private DatabaseContext _context = null;
//        private DbSet<T> table = null;

//		public GenericService(DatabaseContext context)
//		{
//            this.table = _context.Set<T>();
//            this._context = context;
//		}

//        public void Delete(T obj)
//        {
//            T existing = table.Find(id);
//            table.Remove(existing);
//        }

//        public IEnumerable<T> GetAll()
//        {
//            return table.ToList();
//        }

//        public T GetById(int id)
//        {
//            return table.Find(id);
//        }

//        public void Insert(T obj)
//        {
//            table.Add(obj);
//            Save();
//        }

//        public void Save()
//        {
//            _context.SaveChanges();
//        }

//        public void Update(T obj)
//        {
//            table.Attach(obj);
//            _context.Entry(obj).State = EntityState.Modified;
//        }
//    }
//}

