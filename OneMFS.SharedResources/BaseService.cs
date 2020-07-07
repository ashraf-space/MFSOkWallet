using System;
using System.Collections.Generic;
using System.Text;

namespace OneMFS.SharedResources
{
    public interface IBaseService<T>
    {
        T Add(T entity);
        T Update(T entity);
        T UpdateByStringField(T entity, string stringField);
        T UpdateRegInfo(T entity);
        IEnumerable<T> GetAll(T entity);
        T SingleOrDefault(int Id, T entity, string identifier = "Id");
        bool Delete(T entity);
        object GetDropdownList(string label, string value, T entity);
        T SingleOrDefaultByCustomField(string Id, string customField, T entity);
        T DeleteByCustomField(string Id, string customField, T entity);
        IEnumerable<T> GetAllByCustomField(string Id, string customField, T entity);
    }

    public class BaseService<T> 
    {
        public BaseRepository<T> baseRepo = new BaseRepository<T>();

        public T Add(T entity)
        {
            baseRepo.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            baseRepo.Update(entity);
            return entity;
        }

        public T UpdateByStringField(T entity, string stringField)
        {
            baseRepo.UpdateByStringField(entity, stringField);
            return entity;
        }

        public T UpdateRegInfo(T entity)
        {
            baseRepo.UpdateRegInfo(entity);
            return entity;
        }

        public IEnumerable<T> GetAll(T entity)
        {
            return baseRepo.GetAll(entity);
        }

        public T SingleOrDefault(int Id, T entity, string identifier = "Id")
        {
            return baseRepo.SingleOrDefault(Id, entity, identifier);
        }

        public T SingleOrDefaultByCustomField(string Id, string customField, T entity)
        {
            return baseRepo.SingleOrDefaultByCustomField(Id, customField ,entity);
        }

        public T DeleteByCustomField(string Id, string customField, T entity)
        {
            return baseRepo.DeleteByCustomField(Id, customField, entity);
        }

        public bool Delete(T entity)
        {
            return baseRepo.Delete(entity);
        }

        public object GetDropdownList(string label, string value, T entity)
        {
            return baseRepo.GetDropdownList(label, value, entity);
        }

        public IEnumerable<T> GetAllByCustomField(string Id, string customField, T entity)
        {
            return baseRepo.GetAllByCustomField(Id, customField, entity);
        }
    }
}
