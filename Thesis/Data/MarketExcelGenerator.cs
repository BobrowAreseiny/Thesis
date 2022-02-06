using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing.Slicer;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.IO;
using Thesis.Data.Model;

namespace Thesis.Data
{
    public class MarketExcelGenerator
    {
        
        public byte[] Generate(List<UserOrder> report)
        {
            FileInfo template = new FileInfo(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\SaleReport.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage package = new ExcelPackage(null, template);
            ExcelWorksheet sheet = package.Workbook.Worksheets[1];
            int row = 3;

            sheet.Columns.AutoFit();
            sheet.Column(1).Style.Numberformat.Format = "mm.dd.yyyy";
            sheet.Columns.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            foreach (UserOrder item in report)
            {
                sheet.Cells[row, 1].Value = item.DateOfShipment;
                sheet.Cells[row, 2].Value = item.Counterparty.Name;
                sheet.Cells[row, 3].Value = item.Counterparty.Address.Town.TownName;
                sheet.Cells[row, 4].Value = item.TotalAmount;
                row++;
            }

            PivotSlicers(sheet);
            sheet.Columns.AutoFit();
            return package.GetAsByteArray();
        }

        private void PivotSlicers(ExcelWorksheet sheet)
        {
            ExcelWorksheet wsPivot = sheet.Workbook.Worksheets[0];

            ExcelPivotTableSlicer monthSlicer = wsPivot.PivotTables[0].Fields["Месяцы"].AddSlicer();
            monthSlicer.Caption = "Месяцы";
            monthSlicer.ChangeCellAnchor(eEditAs.Absolute);
            monthSlicer.SetPosition(80, 950);
            monthSlicer.SetSize(200, 600);
            monthSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[3].PivotTables[0]);
            monthSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[4].PivotTables[0]);
            monthSlicer.Style = eSlicerStyle.Other2;

            ExcelPivotTableSlicer regionSlicer = wsPivot.PivotTables[0].Fields["регион"].AddSlicer();
            regionSlicer.Caption = "Регионы";
            regionSlicer.ChangeCellAnchor(eEditAs.Absolute);
            regionSlicer.SetPosition(80, 1150);
            regionSlicer.SetSize(200, 200);
            regionSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[2].PivotTables[0]);
            regionSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[3].PivotTables[0]);
            regionSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[5].PivotTables[0]);
            regionSlicer.Style = eSlicerStyle.Other2;

            ExcelPivotTableSlicer organizationSlicer = wsPivot.PivotTables[0].Fields["организация"].AddSlicer();
            organizationSlicer.Caption = "Организации";
            organizationSlicer.ChangeCellAnchor(eEditAs.Absolute);
            organizationSlicer.SetPosition(280, 1150);
            organizationSlicer.SetSize(200, 200);
            organizationSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[2].PivotTables[0]);
            organizationSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[4].PivotTables[0]);
            organizationSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[5].PivotTables[0]);
            organizationSlicer.Style = eSlicerStyle.Other2;

            ExcelPivotTableSlicer yearSlicer = wsPivot.PivotTables[0].Fields["годы"].AddSlicer();
            yearSlicer.Caption = "Годы";
            yearSlicer.ChangeCellAnchor(eEditAs.Absolute);
            yearSlicer.SetPosition(480, 1150);
            yearSlicer.SetSize(200, 200);
            yearSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[2].PivotTables[0]);
            yearSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[3].PivotTables[0]);
            yearSlicer.Cache.PivotTables.Add(sheet.Workbook.Worksheets[4].PivotTables[0]);
            yearSlicer.Style = eSlicerStyle.Other2;
        }

        public byte[] Generate(List<OrderConstruction> report)
        {
            FileInfo template = new FileInfo(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Tovarnaya_nakladnaya.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage package = new ExcelPackage(null, template);
            ExcelWorksheet sheet = package.Workbook.Worksheets[0];

            int row = 11;

            OrderConstructionReportUpperStyle(sheet, report[0].UserOrder);

            foreach (OrderConstruction item in report)
            {
                OrderConstructionReport(sheet, row);
                OrderConstructionReportFormat(sheet, row);
                OrderConstructionReportData(sheet, row, item);
                row++;
            }
            OrderConstructionReportStyle(sheet, row);
            OrderConstructionReportButtomStyle(sheet, row);

            return package.GetAsByteArray();
        }
        private void OrderConstructionReportUpperStyle(ExcelWorksheet sheet, UserOrder order)
        {
            sheet.Cells[2, 1].Value = $"Накладная № {order.OrderNumber} от {DateTime.Now.Date}";
            sheet.Cells[4, 6].Value = "СЗАО Отико";
            sheet.Cells[6, 6].Value = order.Counterparty.Name;
        }
        private void OrderConstructionReport(ExcelWorksheet sheet,int row)
        {
            sheet.Cells[row, 1, row, 2].Merge = true;//№
            sheet.Cells[row, 3, row, 6].Merge = true;//Код
            sheet.Cells[row, 7, row, 14].Merge = true;//Товар
            sheet.Cells[row, 15, row, 21].Merge = true;//Размер
            sheet.Cells[row, 22, row, 27].Merge = true;//Кол-во
            sheet.Cells[row, 28, row, 31].Merge = true;//Цена без ндс 
            sheet.Cells[row, 32, row, 36].Merge = true;//Сумма без ндс
            sheet.Cells[row, 37, row, 41].Merge = true;//Ндс
            sheet.Cells[row, 42, row, 45].Merge = true;//Сумма НДС
            sheet.Cells[row, 46].Merge = true;//Сумма с НДС
        }
        private void OrderConstructionReportFormat(ExcelWorksheet sheet, int row)
        {
            sheet.Cells[row, 24, row, 27].Style.Numberformat.Format = "#";
            sheet.Cells[row, 28, row, 31].Style.Numberformat.Format = "$#,##";
            sheet.Cells[row, 32, row, 36].Style.Numberformat.Format = "$#,##";
            sheet.Cells[row, 42, row, 45].Style.Numberformat.Format = "$#,##";
            sheet.Cells[row, 46].Style.Numberformat.Format = "$#,##";
        }
      
        private void OrderConstructionReportData(ExcelWorksheet sheet, int row, OrderConstruction item)
        {
            sheet.Cells[row, 1].Value = row - 10;
            sheet.Cells[row, 3].Value = item.NumberOfProucts;
            sheet.Cells[row, 7].Value = item.ProductSize.Product.Name;
            sheet.Cells[row, 15].Value = item.ProductSize.Size;
            sheet.Cells[row, 22].Value = item.Amount;
            sheet.Cells[row, 28].Value = item.ProductSize.Product.Price;
            sheet.Cells[row, 32].Formula = $"V{row} * AB{row}";
            sheet.Cells[row, 37].Value = 20;
            sheet.Cells[row, 42].Formula = $"V{row} * AB{row} * 0.2";
            sheet.Cells[row, 46].Formula = $"V{row} * AB{row} * 1.2";
        }

        private void OrderConstructionReportStyle(ExcelWorksheet sheet,int row)
        {
            using (ExcelRange range = sheet.Cells[11, 1, row - 1, 46])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 10;
            };
        }
        private void OrderConstructionReportButtomStyle(ExcelWorksheet sheet, int row)
        {
            using (ExcelRange range = sheet.Cells[row + 1, 25, row + 1, 46])
            {
                range.Style.Font.Bold = true;
                range.Style.Font.Size = 10;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;             
                sheet.Cells[row + 1, 32, row + 1, 45].Merge = true;
                sheet.Cells[row + 2, 1, row + 2, 46].Merge = true;
                sheet.Cells[row + 3, 1, row + 3, 46].Merge = true;
                sheet.Cells[row + 1, 32].Value = $"Итого к оплате: ";
                sheet.Cells[row + 1, 46].Formula = $"=СУММ(AT11:AT{row - 1})";
                sheet.Cells[row + 2, 1, row + 2, 43].Value = $"Всего наименований {row - 11}, на сумму {sheet.Cells[row + 1, 32].Value} руб";
                sheet.Cells[row + 3, 1, row + 3, 43].Value = "Сумма прописью";
            };
        }
    }
}
