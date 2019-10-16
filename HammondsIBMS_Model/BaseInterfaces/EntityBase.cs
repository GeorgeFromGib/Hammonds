using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;

namespace HammondsIBMS_Domain.BaseInterfaces
{
    public abstract class EntityBase : INotifyPropertyChanged
    {
        //[Timestamp]
        //public byte[] TimeStamp { get; set; }
        private class ModifiedField<C> 
        {
            public C OldValue { get; set; }
            public C NewValue { get; set; }

            public ModifiedField(C oldValue, C newValue)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        private Dictionary<string,ModifiedField<object>> _modifiedFields=new Dictionary<string, ModifiedField<object>>(); 

        public bool IsEntityDirty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
          
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            MemberExpression body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            var propertyName = body.Member.Name;
           
            OnPropertyChanged(propertyName);
        }

        protected bool SetValue<T>(ref T field, T value, Expression<Func<T>> selectorExpression)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            var oldValue = field;
            var newValue = value;
            var propertyName = GetPropertyName(selectorExpression);
            field = value;
            IsEntityDirty = true;
            _modifiedFields.Add(propertyName,new ModifiedField<object>(oldValue,newValue));
            
            OnPropertyChanged(propertyName);
            return true;
        }

        private string GetPropertyName<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
                throw new ArgumentNullException("selectorExpression");
            MemberExpression body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            return body.Member.Name;
        }

    }
}
