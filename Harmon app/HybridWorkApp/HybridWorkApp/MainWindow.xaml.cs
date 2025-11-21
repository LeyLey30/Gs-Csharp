using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using HybridWorkApp.Models;
using HybridWorkApp.Services;

namespace HybridWorkApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ScheduleItem> _schedule = new ObservableCollection<ScheduleItem>();
        private PersistenceService _persistence = new PersistenceService();
        private ScheduleService _scheduleService;

        public MainWindow()
        {
            InitializeComponent();

            // populate days
            var days = Enum.GetNames(typeof(DayOfWeek));
            foreach (var d in days) CbDay.Items.Add(d);
            CbDay.SelectedIndex = 0;
            CbType.SelectedIndex = 0;

            GridSchedule.ItemsSource = _schedule;

            // sample data
            _schedule.Add(new ScheduleItem { Day = "Monday", Type = ScheduleType.Presential, Notes = "Escritório - reunião" });
            _schedule.Add(new ScheduleItem { Day = "Tuesday", Type = ScheduleType.Remote, Notes = "Home office - foco" });
            _scheduleService = new ScheduleService(_schedule.ToList());
            UpdateSummary();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CbDay.SelectedItem == null || CbType.SelectedItem == null) return;
            var day = CbDay.SelectedItem.ToString();
            var type = ((ComboBoxItem)CbType.SelectedItem).Content.ToString();
            var item = new ScheduleItem
            {
                Day = day,
                Type = Enum.Parse<ScheduleType>(type, true),
                Notes = ""
            };
            _schedule.Add(item);
            _scheduleService.Update(_schedule.ToList());
            UpdateSummary();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (GridSchedule.SelectedItem is ScheduleItem si)
            {
                _schedule.Remove(si);
                _scheduleService.Update(_schedule.ToList());
                UpdateSummary();
            }
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            var index = _scheduleService.CalculateBalanceIndex();
            TxtProductivity.Text = $"Índice de equilíbrio (0-100): {index:F1}";
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var user = new UserData
            {
                UserName = TxtUserName.Text ?? "Usuário",
                Schedule = _schedule.ToList()
            };
            try
            {
                await _persistence.SaveAsync(user);
                MessageBox.Show("Dados salvos com sucesso.", "Salvar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

        private async void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = await _persistence.LoadAsync();
                if (user != null)
                {
                    TxtUserName.Text = user.UserName;
                    _schedule.Clear();
                    foreach (var s in user.Schedule) _schedule.Add(s);
                    _scheduleService.Update(_schedule.ToList());
                    UpdateSummary();
                    MessageBox.Show("Dados carregados.", "Carregar", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else MessageBox.Show("Nenhum dado encontrado.", "Carregar", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message);
            }
        }

        private void BtnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            var pres = _schedule.Count(s => s.Type == ScheduleType.Presential);
            var rem = _schedule.Count(s => s.Type == ScheduleType.Remote);
            var hyb = _schedule.Count(s => s.Type == ScheduleType.Hybrid);
            TxtSummary.Text = $"Usuário: {TxtUserName.Text}\nDias presenciais: {pres}\nDias remotos: {rem}\nDias híbridos: {hyb}\nTotal: {_schedule.Count}";
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Day,Type,Notes");
            foreach (var s in _schedule)
            {
                sb.AppendLine($"{s.Day},{s.Type},{s.Notes}");
            }
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "cronograma_export.csv");
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Exportado para: " + path);
        }
    }
}
