using Core.Common.Contracts;
using Core.Common.Extensions;
using Core.Common.Utils;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        #region Fields

        private bool _isDirty;
        protected IValidator _validator;
        protected IList<ValidationFailure> _errors = new List<ValidationFailure>();

        #endregion

        #region Properties

        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors { get { return _errors; } }
        [NotNavigable]
        public bool IsValid { get { return !(_errors.Any()); } }

        #endregion

        #region Constructor

        public ObjectBase()
        {
            _validator = GetValidator();
            Validate();
        }

        #endregion

        #region INotifyPropertyChanged

        protected override void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                _isDirty = true;

            Validate();
        }

        protected override void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region IDirtyCapable

        [NotNavigable]
        public bool IsDirty
        {
            get { return _isDirty; }
            protected set
            {
                _isDirty = value;
                OnPropertyChanged("IsDirty", false);
            }
        }

        public IEnumerable<IDirtyCapable> GetDirtyObjects()
        {
            var dirtyObjects = new List<ObjectBase>();

            WalkObjectGraph((o) =>
            {
                if (o.IsDirty)
                    dirtyObjects.Add(o);

                return false;
            }, null, null);

            return dirtyObjects;
        }

        public void CleanAllDirty()
        {
            WalkObjectGraph((o) =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;

                return false;
            }, null, null);
        }

        public bool IsAnyDirty()
        {
            var isDirty = false;

            WalkObjectGraph((o) =>
            {
                if (o.IsDirty)
                    isDirty = true;

                return o.IsDirty;
            }, null, null);

            return isDirty;
        }

        #endregion

        #region IDataErrorInfo

        string IDataErrorInfo.Error { get { return string.Empty; } }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var error in _errors)
                {
                    if (error.PropertyName == columnName)
                        sb.AppendLine(error.ErrorMessage);
                }

                return sb.ToString();
            }
        }

        #endregion

        #region IExtensibleDataObject
        
        public ExtensionDataObject ExtensionData { get; set; }

        #endregion

        #region PublicMethods

        public void Validate()
        {
            var results = _validator.Validate(this);
            _errors = results.Errors;
        }

        #endregion

        #region AbstractMethods

        protected abstract IValidator GetValidator();

        #endregion

        #region PrivateMethods

        private void WalkObjectGraph(Predicate<ObjectBase> snippetForObject, Action<IList> snippetForCollection, params string[] exemptProperties)
        {
            var visited = new List<ObjectBase>();
            var exemptions = new List<string>();
            exemptions.AddRange(exemptProperties?.ToList());
            Action<ObjectBase> walk = null;

            walk = (o) =>
            {
                if (o == null || visited.Contains(o))
                    return;

                visited.Add(o);
                
                if (snippetForObject(o))
                    return;

                var properties = typeof(ObjectBase).GetProperties()
                    .Where(p => p.IsNavigable())
                    .Where(p => exemptions.Contains(p.Name));
                foreach (var prop in properties)
                {
                    if (prop.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                    {
                        var subO = (ObjectBase)prop.GetValue(o);
                        walk(subO);
                    }
                    else
                    {
                        IList col = null;
                        if ((col = prop.GetValue(o) as IList) != null)
                        {
                            snippetForCollection?.Invoke(col);

                            foreach (var item in col)
                            {
                                if (item is ObjectBase)
                                    walk((ObjectBase)item);
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        #endregion
    }
}
