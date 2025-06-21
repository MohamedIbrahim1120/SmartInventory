using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.DTOs;

namespace SmartInventory.Reports
{
    public class ProductReportPdfGenerator
    {
        public static byte[] Generate(List<ProductDto> products)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("SmartInventory - Product Report")
                        .SemiBold().FontSize(18).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Name
                                columns.RelativeColumn();   // Price
                                columns.RelativeColumn();   // Qty
                                columns.RelativeColumn();   // Category
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Name");
                                header.Cell().Element(CellStyle).Text("Price");
                                header.Cell().Element(CellStyle).Text("Quantity");
                                header.Cell().Element(CellStyle).Text("Category");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten2);
                                }
                            });

                            foreach (var p in products)
                            {
                                table.Cell().Padding(5).Text(p.Name);
                                table.Cell().Padding(5).Text($"{p.Price:C}");
                                table.Cell().Padding(5).Text(p.Quantity.ToString());
                                table.Cell().Padding(5).Text(p.CategoryName ?? "-");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                });
            }).GeneratePdf();
        }
    }
}
