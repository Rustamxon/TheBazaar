﻿using TheBazaar.Domain.Commons;

namespace TheBazaar.Domain.Entities;

public class Product : Auditable
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> SearchTags { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public long CategoryId { get; set; }
}
