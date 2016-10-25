using DTO.DTO.Params;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SgqSystem.ViewModels
{
    public class ParamsViewModel : IValidatableObject
    {
        #region Constructors

        /// <summary>
        /// Construtor para o MVC
        /// </summary>
        public ParamsViewModel() { }

       /// <summary>
       /// Construtor Padrão.
       /// </summary>
       /// <param name="paramsDdl"></param>
        public ParamsViewModel(ParamsDdl paramsDdl)
        {
            this.paramsDdl = paramsDdl;
            paramsDto = new ParamsDTO();
            paramsDto.parLevel1Dto = new ParLevel1DTO();
            paramsDto.parLevel2Dto = new ParLevel2DTO();
            paramsDto.parLevel1XClusterDto = new List<ParLevel1XClusterDTO>();
        }

        /// <summary>
        /// Construtor para View Model Level1
        /// </summary>
        /// <param name="paramsDdl"></param>
        /// <param name="paramsDb"></param>
        public ParamsViewModel(ParamsDdl paramsDdl, ParamsDTO paramsDb)
        {
        }

        #endregion

        public ParamsDTO paramsDto { get; set; }
        public ParamsDdl paramsDdl { get; set; }

        public string pontosCluster { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var i in paramsDdl.DdlparCluster)
            {
                if (i.Selected == true && i.Value.Equals("-1") && string.IsNullOrEmpty(pontosCluster))
                {
yield return new ValidationResult("Description must be supplied.");
                }
            }
        }

    }
}


//public class RequiredIfAttribute : RequiredAttribute
//{
//    private string PropertyName { get; set; }
//    private object DesiredValue { get; set; }

//    public RequiredIfAttribute(string propertyName, object desiredvalue)
//    {
//        PropertyName = propertyName;
//        DesiredValue = desiredvalue;
//    }

//    protected override ValidationResult IsValid(object value, ValidationContext context)
//    {
//        object instance = context.ObjectInstance;
//        Type type = instance.GetType();
//        object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
//        if (proprtyvalue.ToString() == DesiredValue.ToString())
//        {
//            ValidationResult result = base.IsValid(value, context);
//            return result;
//        }
//        return ValidationResult.Success;
//    }
//}