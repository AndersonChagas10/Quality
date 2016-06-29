using Dominio.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityConfig
{
    public class UserConfig : EntityTypeConfiguration<User>
    {

        public UserConfig()
        {


            Property(x => x.Name)
                .HasColumnName("Id")
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("Id_Usuario", 2) { IsUnique = true }));

            //Property(x => x.Name)
            //    .HasColumnName("UserName")
            //    .HasMaxLength(User.NameOrPassMaxValue)
            //    .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("Usuario", 2) { IsUnique = true }));



        }

    }
}
