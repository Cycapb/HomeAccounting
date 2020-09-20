using System.Collections.Generic;
using DomainModels.Model;

namespace WebUI.Models.CategoryModels
{
    public class CategoriesCollectionModel
    {
        public List<Category> Categories { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public int TypeOfFlowId { get; set; }
    }
}