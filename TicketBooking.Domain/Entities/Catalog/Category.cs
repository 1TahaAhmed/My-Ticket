using System;
using System.Collections.Generic;
using System.Text;
using TicketBooking.Domain.BaseEntity;

namespace TicketBooking.Domain.Entities.Catalog
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;

        public int? ParentCategoryId { get; private set; }
        public Category? ParentCategory { get; private set; }

        public string Slug { get; private set; } = string.Empty;

        // Encapsulated Collection
        private readonly List<Category> _subCategories = new();
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();
        
        private Category() { }

        public Category(string name, string description = "", int? parentCategoryId = null)
        {
            UpdateDetails(name, description);
            ParentCategoryId = parentCategoryId;
        }

        public void UpdateDetails(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name can't be null", nameof(name));

            Name = name.Trim();
            Description = description?.Trim() ?? string.Empty;

            Slug = Name.ToLowerInvariant().Replace(" ", "-");
        }
        
        public void AddSubCategory(Category category) 
        {
            ArgumentNullException.ThrowIfNull(category);
            _subCategories.Add(category);
        }

        public void ChangeParent(int? parentCategoryId)
        {
            if(parentCategoryId.HasValue && parentCategoryId.Value == Id)
                throw new InvalidOperationException("A category cannot be its own parent.");
            
            ParentCategoryId = parentCategoryId;
        }
    }
}
