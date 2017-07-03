using System.Collections.Generic;

namespace MS.WebSolutions.DioKft.DataAccessLayer.Entities
{
    public class Category : EntityBase
    {
        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }

    }
}
