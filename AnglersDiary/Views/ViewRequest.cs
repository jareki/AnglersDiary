using AnglersDiary.Models;
using AnglersDiary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnglersDiary.Views
{
    public static class ViewRequest
    {
        public static void AddCatch(AddCatchViewModel vm)
        {
            AddCatchWindow wnd = new AddCatchWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        public static void EditCatch(EditCatchViewModel vm)
        {
            EditCatchWindow wnd = new EditCatchWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        public static void ShowTackles()
        {
            var wnd = new TackleWindow();
            wnd.ShowDialog();
        }

        public static void ShowTrophies()
        {
            TrophyWindow wnd = new TrophyWindow();
            wnd.ShowDialog();
        }

        public static void ShowLocations()
        {
            var wnd = new LocationWindow();
            wnd.Height = SystemParameters.PrimaryScreenHeight * 0.8;
            wnd.Width = SystemParameters.PrimaryScreenWidth * 0.8;
            wnd.ShowDialog();
        }

        public static void AddTrophy(AddTrophyViewModel vm)
        {
            AddTrophyWindow wnd = new AddTrophyWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        public static void EditTrophy(EditTrophyViewModel vm)
        {
            EditTrophyWindow wnd = new EditTrophyWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        public static void ShowImage(string image)
        {
            ImageWindow wnd = new ImageWindow("", new Photo(image));
            wnd.ShowDialog();
        }

        internal static void AddSpecy(AddSpecyViewModel vm)
        {
            AddSpecyWindow wnd = new AddSpecyWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        internal static void AddTackleCategory(AddTackleCategoryViewModel vm)
        {
            AddTackleCategoryWindow wnd = new AddTackleCategoryWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        internal static void EditSpecy(EditSpecyViewModel vm)
        {
            EditSpecyWindow wnd = new EditSpecyWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        internal static Tackle AddNoteTackle()
        {
            TackleWindow wnd = new TackleWindow(true);
            wnd.ShowDialog();
            return wnd.SelectedTackle;
        }

        public static void ShowNote(Note note)
        {
            NoteDoc wnd = new NoteDoc(note);
            wnd.ShowDialog();
        }

        public static void AddTackle(AddTackleViewModel vm)
        {
            AddTackleWindow wnd = new AddTackleWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        internal static void EditNoteTackle(EditNoteTackleViewModel vm)
        {
            EditNoteTackleWindow wnd = new EditNoteTackleWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();

        }

        internal static void EditTackleCategory(EditTackleCategoryViewModel vm)
        {
            EditTackleCategoryWindow wnd = new EditTackleCategoryWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }

        public static void EditTackle(EditTackleViewModel vm)
        {
            EditTackleWindow wnd = new EditTackleWindow();
            vm.RequestClose += (s, e) => wnd.Close();
            wnd.DataContext = vm;
            wnd.ShowDialog();
        }
    }
}
