namespace ArmChair
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ISession : IDisposable
    {
        void Add<T>(T instance) where T : class;
        void Update<T>(T instance) where T : class;
        void Remove<T>(T instance) where T : class;

        IEnumerable<T> GetByIds<T>(IEnumerable ids) where T : class;
        T GetById<T>(object id) where T : class;

        void Commit();
    }
}