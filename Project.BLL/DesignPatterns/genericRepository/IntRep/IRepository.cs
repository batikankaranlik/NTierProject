using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DesignPatterns.genericRepository.IntRep
{
    public interface IRepository<T> where T:BaseEntity
    {
        //List Commands
        List<T> GetAll(); //Bütün bilgileri listelemek
        List<T> GetActives(); //Sadece aktif verileri listelemek
        List<T> GetPassives(); //Sadece Pasif Verileri listelemek
        List<T> GetModifieds(); //Sadece güncellenmiş verileri listelemek

        //Modify Commands
        void Add(T item);//Ekleme metodu
        void AddRange(List<T> item);//çoklu Ekleme metodu
        void Delete(T item);//pasife çekme
        void DeleteRange(List<T> item);// çoklu pasife çekme
        void Update(T item);//Veriyi güncelleme
        void UpdateRange(List<T> item);//çoklu Veriyi güncelleme
        void Destroy(T item);//veriyi yok etme  
        void DestroyRange(List<T> item);//çoklu veriyi yok etme  

        //Linq Expressions
        List<T> Where(Expression<Func<T,bool>> exp);
        bool Any(Expression<Func<T,bool>> exp);
        T FirstOrDefault(Expression<Func<T,bool>> exp);
        object Select(Expression<Func<T,object>> exp);

        //Find
        T Find(int id);

        T FindFirstData();
        T FindLastData();



    }
}
