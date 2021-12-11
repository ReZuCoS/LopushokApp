using System;
using System.Collections.Generic;

namespace LopushokApp.Model
{
    class NormilizedProduct
    {
        public NormilizedProduct(Product product, LopushokDatabaseModel database)
        {
            ID = product.ID;
            Title = product.Title;
            ProductTypeID = product.ProductTypeID;
            ArticleNumber = product.ArticleNumber;
            Description = product.Description;
            Image = product.Image;
            ProductionPersonCount = product.ProductionPersonCount;
            ProductionWorkshopNumber = product.ProductionWorkshopNumber;
            MinCostForAgent = product.MinCostForAgent;
            ProductType = product.ProductType;
            ProductMaterials = product.ProductMaterials;
            NormilizedImage = $@"{Environment.CurrentDirectory}\Resources{this.Image}";

            string tempList = "";

            foreach (ProductMaterial productMaterial in ProductMaterials)
            {
                Material material = database.Materials.Find(productMaterial.MaterialID);

                tempList = $"{tempList}\n{material.Title}";

                Cost = Convert.ToDecimal(Convert.ToDouble(material.Cost) * productMaterial.Count);
            }

            NormilizedProductType = database.ProductTypes.Find(product.ProductTypeID).Title;

            MaterialsList = tempList;

            NormilizedCost = $"{Cost}.00 руб.";
        }

        public int ID { get; set; }

        public string Title { get; set; }

        public int? ProductTypeID { get; set; }

        public string ArticleNumber { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public int? ProductionPersonCount { get; set; }

        public int? ProductionWorkshopNumber { get; set; }

        public decimal MinCostForAgent { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }

        public string NormilizedProductType { get; set; }

        public string NormilizedImage { get; set; }

        public string MaterialsList { get; set; }

        public decimal Cost { get; set; }

        public string NormilizedCost { get; set; }
    }
}
