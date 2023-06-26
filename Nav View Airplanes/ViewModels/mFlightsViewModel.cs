using DevExpress.Mvvm;
using Nav_View_Airplanes.Models;
using Nav_View_Airplanes.Services;
using Nav_View_Airplanes.Views;
using System.Collections.Generic;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Windows.Media.Media3D;
using System;

namespace Nav_View_Airplanes.ViewModels
{
    public  class mFlightsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly GetService _getService;

        public List<Flight> Flights { get;set; } 
        
        public mFlightsViewModel(PageService pageService, GetService getService)
        {
            _pageService = pageService;
            _getService = getService;
            Load();
        }
        public async void Load()
        {
            var context = await _getService.GetFlights();
            Flights = context;
        }
        public DelegateCommand GetFile => new(() =>
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream("flight.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, 10, Font.NORMAL);

            PdfPTable table = new PdfPTable(9);
            table.WidthPercentage = 100;
            PdfPCell cell0 = new PdfPCell(new Phrase("Список рейсов", font));
            cell0.HorizontalAlignment = 1;
            cell0.Colspan = 9;
            cell0.Border = 0;
            table.AddCell(cell0);
            PdfPCell cell = new PdfPCell(new Phrase("Номер рейса", font));
            cell.HorizontalAlignment = 1;
            cell.Rowspan = 2;
            table.AddCell(cell);
            PdfPCell cell2 = new PdfPCell(new Phrase("Отбытие", font));
            cell2.HorizontalAlignment = 1;
            cell2.Colspan = 3;
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Прибытие", font));
            cell3.HorizontalAlignment = 1;
            cell3.Colspan = 3;
            table.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Самолет", font));
            cell4.HorizontalAlignment = 1;
            cell4.Rowspan = 2;
            table.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Статус рейса", font));
            cell5.HorizontalAlignment = 1;
            cell5.Rowspan = 2;
            table.AddCell(cell5);

            PdfPCell cell6 = new PdfPCell(new Phrase("Время", font));
            cell6.HorizontalAlignment = 1;
            PdfPCell cell7 = new PdfPCell(new Phrase("Дата", font));
            cell7.HorizontalAlignment = 1;
            PdfPCell cell8 = new PdfPCell(new Phrase("Город", font));
            cell8.HorizontalAlignment = 1;

            table.AddCell(cell6);
            table.AddCell(cell7);
            table.AddCell(cell8);
            table.AddCell(cell6);
            table.AddCell(cell7);
            table.AddCell(cell8);

            foreach (Flight flight in Flights)
            {
                table.AddCell(new Phrase(flight.Idflight.ToString(), font));
                table.AddCell(new Phrase(flight.DepartureTime.ToString("t"), font));
                table.AddCell(new Phrase(flight.DepartureTime.Date.ToString("d"), font));
                table.AddCell(new Phrase(flight.DepartureAirportNavigation.City, font));
                table.AddCell(new Phrase(flight.ArrivalTime.ToString("t"), font));
                table.AddCell(new Phrase(flight.ArrivalTime.Date.ToString("d"), font));
                table.AddCell(new Phrase(flight.ArrivalAirportNavigation.City, font));
                table.AddCell(new Phrase(flight.IdplaneNavigation.Model, font));
                table.AddCell(new Phrase(flight.StatusNavigation.FlightStatus1, font));
            }
            PdfPCell cell9 = new PdfPCell(new Phrase("Дата создания документа: " + DateTime.Now.ToString("g"), font));
            cell9.Colspan = 9;
            cell9.HorizontalAlignment = 2;
            cell9.Border = 0;
            table.AddCell(cell9);


            doc.Add(table);
            doc.Close();
            var p = new Process();
            p.StartInfo = new ProcessStartInfo("flight.pdf")
            {
                UseShellExecute = true
            };
            p.Start();
        });
        public DelegateCommand Back => new(() => _pageService.ChangePage(new MainPage()));

    }
}
