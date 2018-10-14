using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AnglersDiary.Views
{
    /// <summary>
    /// Interaction logic for NoteDoc.xaml
    /// </summary>
    public partial class NoteDoc
    {
        public Note Note { get; set; }
        public List<Note> Notes { get; set; }

        public NoteDoc(Note note)
        {
            Note = note;
            Notes = new List<Note>
            {
                note
            };

            InitializeComponent();
        }

        public NoteDoc(List<Note> notes)
        {
            Notes = new List<Note>();
            notes.ForEach(n => Notes.Add(n));

            InitializeComponent();
            EditBtn.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateTxt.Text = $"{Note.Date:dd.MM.yyyy}";
            TimeTxt.Text = $"{Note.StartTime:HH:mm} - {Note.EndTime:HH:mm}";

            #region Location
                LocationTxt.Text = $"{Note.Location?.Name}";
                LatitudeTxt.Text = $"{Note.Location?.Latitude:##.####}";
                LongitudeTxt.Text = $"{Note.Location?.Longitude:##.####}";
            #endregion

            #region Trophies
            var trophies = new ShowAllTrophiesViewModel(Note).Trophies;
            if (trophies.Count() > 0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    TableColumn column = new TableColumn();
                    column.Width = new GridLength(100);
                    TrophyTable.Columns.Add(column);
                }
                var rowgroup = new TableRowGroup();

                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run("Фото"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Вид"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Вес"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Длина"))));
                rowgroup.Rows.Add(row);

                foreach (var trophy in trophies)
                {
                    row = new TableRow();

                    var img = new Image();
                    img.Source = new Photo(trophy.Image).Thumbnail;
                    img.Width = 100;
                    img.Height = 100;
                    row.Cells.Add(new TableCell(new BlockUIContainer(img)));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(trophy.Specy.Name))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{trophy.Weight} гр"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{trophy.Size} см"))));
                    rowgroup.Rows.Add(row);
                }
                TrophyTable.RowGroups.Add(rowgroup);
            }
            #endregion

            #region Catches

            var catches = new ShowAllCatchesViewModel(Note).Catches;
            if (catches.Count() > 0)
            {
                for (int i = 0; i <= 3; i++)
                {
                    TableColumn column = new TableColumn();
                    column.Width = new GridLength(150);
                    CatchTable.Columns.Add(column);
                }
                TableRowGroup rowgroup = new TableRowGroup();

                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run("Вид"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Количество"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Параметр"))));
                rowgroup.Rows.Add(row);

                foreach (var catchy in catches)
                {
                    row = new TableRow();

                    row.Cells.Add(new TableCell(new Paragraph(new Run(catchy.Specy.Name))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"{catchy.Count} шт"))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(catchy.Param))));
                    rowgroup.Rows.Add(row);
                }
                CatchTable.RowGroups.Add(rowgroup);
            }
            #endregion

            #region NoteTackles

            var notetackles = new ShowAllNoteTacklesViewModel(Note).NoteTackles;
            if (notetackles.Count() > 0)
            {
                for (int i = 0; i <= 4; i++)
                {
                    TableColumn column = new TableColumn();
                    column.Width = new GridLength(150);
                    TackleTable.Columns.Add(column);
                }
                var rowgroup = new TableRowGroup();

                TableRow row = new TableRow();
                row.Cells.Add(new TableCell(new Paragraph(new Run("Фото"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Категория"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Название"))));
                row.Cells.Add(new TableCell(new Paragraph(new Run("Параметр"))));
                rowgroup.Rows.Add(row);

                foreach (var notetackle in notetackles)
                {
                    row = new TableRow();
                    var img = new Image();
                    img.Source = new Photo(notetackle.Tackle.Image).Thumbnail;
                    img.Width = 100;
                    img.Height = 100;
                    row.Cells.Add(new TableCell(new BlockUIContainer(img)));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(notetackle.Tackle.TackleCategory.Name))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(notetackle.Tackle.Name))));
                    row.Cells.Add(new TableCell(new Paragraph(new Run(notetackle.Parameter))));
                    rowgroup.Rows.Add(row);
                }
                TackleTable.RowGroups.Add(rowgroup);
            }
            #endregion

            #region Weather Params
            string temp_min = Note.TempMin < 0 ? $"-{Note.TempMin}" : $"+{Note.TempMin}";
            string temp_max = Note.TempMax < 0 ? $"-{Note.TempMax}" : $"+{Note.TempMax}";
            TempTxt.Text = $"T = {temp_min} {temp_max} C";
            WaterTempTxt.Text = $"T воды = {Note.TempWater}C";
            PressureTxt.Text = $"{Note.Pressure} мм";
            WindTxt.Text = $"{Note.WindDir}, {Note.WindSpeed} м/с";
            if (Note.Rainfall)
                CloudTxt.Text = $"{Note.Cloud}, осадки";
            else
                CloudTxt.Text = $"{Note.Cloud}";
            MoonTxt.Text = $"{Note.Moon}";
            WeatherNoteTxt.Text = $"{Note.WeatherNote}";
            #endregion

            #region Water Params
            WaterTranspTxt.Text = $"Прозрачность: {Note.WaterTransparency}";
            FlowTxt.Text = $"Течение: {Note.Flow}";
            DepthTxt.Text = $"Глубина: {Note.Depth:##.#} м";
            BottomTxt.Text = $"Дно: {Note.Bottom}";
            WaterHeightTxt.Text = $"Уровень воды: {Note.WaterHeight}";
            WasteTxt.Text = $"Мусор: {Note.Waste}";
            DistanceTxt.Text = $"Дальность: {Note.Distance} м";
            WaterNoteTxt.Text = $"{Note.WaterNote}";
            #endregion

            #region Catch Params
            CatchCountTxt.Text = $"Количество: {Note.CatchCount} штук";
            CatchWeightTxt.Text = $"Вес: {Note.CatchWeight:##.#} кг";

            NoteTxt.Text = $"{Note.NoteString}";
            #endregion
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var content = new TextRange(DocViewer.ContentStart, DocViewer.ContentEnd);

            var dlg = new SaveFileDialog();
            dlg.Filter = "Rft-files (*.rtf)|*.rtf";
            dlg.FileName = Note.Date.ToString("dd-MM-yy");
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (dlg.ShowDialog() == true)
                if (content.CanSave(DataFormats.Rtf))
                {
                    try
                    {
                        using (FileStream fs = File.Open(dlg.FileName, FileMode.Create))
                        {
                            content.Save(fs, DataFormats.Rtf);
                        }
                    }
                    catch (IOException)
                    { MessageBox.Show("Не удалось сохранить файл"); }
                }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteViewModel model = new NoteViewModel();
            model.EditCommand.Execute(model.Notes.FirstOrDefault(n => n.Id == Note.Id));
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            NoteViewModel model = new NoteViewModel();
            model.DeleteCommand.Execute(model.Notes.FirstOrDefault(n => n.Id == Note.Id));
            Close();
        }

        private void LocationPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            ImageGalleryWindow wnd = new ImageGalleryWindow($"{App.FolderName}\\Assets\\notes\\{Note.Id}\\photos");
            wnd.ShowDialog();
        }

        private void CatchPhotoBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            MemoryStream stream = new MemoryStream();
            TextRange source = new TextRange(DocViewer.ContentStart, DocViewer.ContentEnd);
            source.Save(stream, DataFormats.XamlPackage);
            FlowDocument copy = new FlowDocument();
            TextRange dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(stream, DataFormats.XamlPackage);

            PrintDialog dlg = new PrintDialog();
            if (dlg.ShowDialog()==true)
            {
                copy.PageWidth = dlg.PrintableAreaWidth;
                copy.ColumnWidth = copy.PageWidth;
                copy.PageHeight = dlg.PrintableAreaHeight;
                dlg.PrintDocument(((IDocumentPaginatorSource)copy).DocumentPaginator,Note.Date.ToString("dd-MM-yy"));
            }
        }

        
    }
}
