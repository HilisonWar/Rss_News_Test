using System;
using System.Collections.Generic;

namespace Rss_Application.Models;

public partial class News
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public string? Url { get; set; }

    public DateTime? AddingDate { get; set; }
}
