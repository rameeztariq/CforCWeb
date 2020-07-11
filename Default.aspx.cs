using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using LumenWorks.Framework.IO.Csv;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetRecords();
    }

    private void GetRecords()
    {
        var csvTable = new DataTable();
        string filePath = @"C:\Users\rameezt\source\repos\CforCWeb\CforCWeb\c4c.csv";
        using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(filePath)), true))
        {
            csvTable.Load(csvReader);
        }
            
        var groupedData = from b in csvTable.AsEnumerable()
                          group b by b.Field<string>("violation_category") into g
                          let list = g.ToList()
                          select new
                          {
                              Violation_Category = g.Key,
                              Violation_Count = list.Count,
                              LatestViolationDate = g.Max(e => e.Field<string>("violation_date")),
                              EarliestViolationDate = g.Min(e => e.Field<string>("violation_date"))
                          };

        Response.Write("<strong>Violations by Category</strong><br>");
        for(int i=0;i < groupedData.Count(); i++)
        {            
            Response.Write("<br>Category:" + groupedData.ElementAt(i).Violation_Category + " has " + groupedData.ElementAt(i).Violation_Count + " violations. The first violation was on "+ groupedData.ElementAt(i).EarliestViolationDate +". The latest violation was on " + groupedData.ElementAt(i).LatestViolationDate+". <br>");
        }        
        
    }
}