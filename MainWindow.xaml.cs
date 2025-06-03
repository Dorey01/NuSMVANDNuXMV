using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NuXMVRunner
{
    public partial class MainWindow : Window
    {
        private List<string> ltlNames = new List<string>();
        private List<(string State, Dictionary<string, string> Data, bool IsLoopStart)> traceData = new List<(string, Dictionary<string, string>, bool)>();
        private HashSet<string> variables = new HashSet<string>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateLog("Logs will appear here...\n");
        }

        // Обработчик кнопки Browse
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "NuXMV/SMV Files (*.nuxmv;*.smv)|*.nuxmv;*.smv|All Files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = dialog.FileName;
            }
        }

        // Обработчик кнопки Apply
        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text.Trim();

            if (string.IsNullOrEmpty(filePath))
            {
                UpdateLog("Error: Please enter a file path.\n");
                return;
            }

            if (!File.Exists(filePath))
            {
                UpdateLog($"Error: File '{filePath}' does not exist.\n");
                return;
            }

            UpdateLog($"Running NuXMV on file: {filePath}\n");
            ApplyButton.IsEnabled = false;

            await Task.Run(() => ExecuteNuXMV(filePath));
            ApplyButton.IsEnabled = true;

            if (ltlNames.Any())
            {
                ShowLTLButtons();
            }
        }

        // Запуск NuXMV
        private void ExecuteNuXMV(string filePath)
        {
            try
            {
                // Чтение файла для поиска LTLSPEC
                string content = File.ReadAllText(filePath);
                ltlNames = Regex.Matches(content, @"LTLSPEC\s+NAME\s+(\w+)")
                    .Cast<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToList();

                if (!ltlNames.Any())
                {
                    UpdateLog("No LTLSPEC NAME found in file.\n");
                    return;
                }

                // Динамически определяем путь к nuxmv.exe с учетом подпапки
                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string directory = Path.GetDirectoryName(assemblyPath);
                string nuxmvPath = Path.Combine(directory, "nuXmv-2.1.0-win64", "bin", "nuxmv.exe");

                if (!File.Exists(nuxmvPath))
                {
                    UpdateLog($"Error: nuxmv.exe not found at {nuxmvPath}. Please ensure it is in the nuXmv-2.1.0-win64/bin subdirectory of the application directory.\n");
                    return;
                }

                var processInfo = new ProcessStartInfo
                {
                    FileName = nuxmvPath,
                    Arguments = $"-int \"{filePath}\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    process.StandardInput.WriteLine("go");
                    process.StandardInput.Close();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        UpdateLog("NuXMV Output:\n" + output);
                    }
                    if (!string.IsNullOrEmpty(error))
                    {
                        UpdateLog("NuXMV Errors:\n" + error);
                    }

                    UpdateLog("NuXMV started successfully.\n");
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Error: {ex.Message}\n");
            }
        }

        private void ShowLTLButtons()
        {
            var window = new Window
            {
                Title = "Select LTL Specification",
                Width = 300,
                Height = 400,
                Content = new ScrollViewer
                {
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10)
                    }
                }
            };

            var stackPanel = (StackPanel)((ScrollViewer)window.Content).Content;

            // Добавляем кнопку "Проверить все" первой
            var checkAllButton = new Button
            {
                Content = "All",
                Margin = new Thickness(0, 2, 0, 2),
                Padding = new Thickness(5)
            };
            checkAllButton.Click += async (s, e) =>
            {
                foreach (var name in ltlNames)
                {
                    await RunLTLCheck(name);
                }
            };
            stackPanel.Children.Add(checkAllButton);

            // Добавляем кнопки для отдельных свойств LTLSPEC
            foreach (string name in ltlNames)
            {
                var button = new Button
                {
                    Content = name,
                    Margin = new Thickness(0, 2, 0, 2),
                    Padding = new Thickness(5)
                };
                button.Click += async (s, e) => await RunLTLCheck(name);
                stackPanel.Children.Add(button);
            }

            window.Show();
        }

        // Проверка LTLSPEC формулы
        private async Task RunLTLCheck(string name)
        {
            string filePath = FilePathTextBox.Text.Trim();
            UpdateLog($"Checking LTL property: {name}\n");

            try
            {
                // Динамически определяем путь к nuxmv.exe с учетом подпапки
                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string directory = Path.GetDirectoryName(assemblyPath);
                string nuxmvPath = Path.Combine(directory, "nuXmv-2.1.0-win64", "bin", "nuxmv.exe");

                if (!File.Exists(nuxmvPath))
                {
                    UpdateLog($"Error: nuxmv.exe not found at {nuxmvPath}. Please ensure it is in the nuXmv-2.1.0-win64/bin subdirectory of the application directory.\n");
                    return;
                }

                var processInfo = new ProcessStartInfo
                {
                    FileName = nuxmvPath,
                    Arguments = $"-int \"{filePath}\"",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    process.StandardInput.WriteLine($"go\ncheck_ltlspec -P {name}\nquit");
                    process.StandardInput.Close();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                    {
                        UpdateLog($"\n--- Output for {name} ---\n" + output);
                        ParseTrace(output);
                    }
                    if (!string.IsNullOrEmpty(error))
                    {
                        UpdateLog($"\n--- Errors for {name} ---\n" + error);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Error: {ex.Message}\n");
            }
        }

        // Парсинг трассировки
        private void ParseTrace(string output)
        {
            traceData.Clear();
            variables.Clear();

            int traceStart = output.IndexOf("Trace Description: LTL Counterexample");
            if (traceStart == -1)
                return;

            string[] traceLines = output.Substring(traceStart).Split('\n');
            string currentState = null;
            Dictionary<string, string> stateData = new Dictionary<string, string>();
            bool isLoopStart = false;

            foreach (string line in traceLines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    continue;

                if (trimmedLine.Contains("-- Loop starts here"))
                {
                    isLoopStart = true;
                    continue;
                }

                var stateMatch = Regex.Match(trimmedLine, @"-> State: (\d+\.\d+) <-");
                if (stateMatch.Success)
                {
                    if (currentState != null && stateData.Any())
                    {
                        traceData.Add((currentState, new Dictionary<string, string>(stateData), isLoopStart));
                        isLoopStart = false;
                    }
                    currentState = $"State {stateMatch.Groups[1].Value}";
                    stateData.Clear();
                    continue;
                }

                var varMatch = Regex.Match(trimmedLine, @"(\w+)\s*=\s*(TRUE|FALSE|\w+)");
                if (varMatch.Success && currentState != null)
                {
                    string varName = varMatch.Groups[1].Value;
                    string varValue = varMatch.Groups[2].Value;
                    stateData[varName] = varValue;
                    variables.Add(varName);
                }
            }

            if (currentState != null && stateData.Any())
            {
                traceData.Add((currentState, new Dictionary<string, string>(stateData), isLoopStart));
            }

            if (traceData.Any())
            {
                DisplayTrace();
            }
        }

        // Отображение трассировки
        private void DisplayTrace()
        {
            // Первая таблица: исходные данные с пропусками
            var originalWindow = new Window
            {
                Title = "Trace Viewer (Original)",
                Width = 800,
                Height = 400
            };

            var originalDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                IsReadOnly = true
            };

            // Вторая таблица: заполненные данные
            var filledWindow = new Window
            {
                Title = "Trace Viewer (Filled)",
                Width = 800,
                Height = 400
            };

            var filledDataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserAddRows = false,
                CanUserDeleteRows = false,
                IsReadOnly = true
            };

            // Создание столбцов для обеих таблиц
            originalDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Variable",
                Binding = new Binding("Variable"),
                Width = 100
            });

            filledDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Variable",
                Binding = new Binding("Variable"),
                Width = 100
            });

            for (int i = 0; i < traceData.Count; i++)
            {
                string stateName = traceData[i].State;
                var originalColumn = new DataGridTextColumn
                {
                    Header = stateName,
                    Binding = new Binding($"Values[{i}]"),
                    Width = 100
                };

                var filledColumn = new DataGridTextColumn
                {
                    Header = stateName,
                    Binding = new Binding($"FilledValues[{i}]"),
                    Width = 100
                };

                // Подсветка столбца с -- Loop starts here
                if (traceData[i].IsLoopStart)
                {
                    var style = new Style(typeof(DataGridCell));
                    style.Setters.Add(new Setter(BackgroundProperty, Brushes.LightYellow));
                    originalColumn.CellStyle = style;
                    filledColumn.CellStyle = style;
                }

                originalDataGrid.Columns.Add(originalColumn);
                filledDataGrid.Columns.Add(filledColumn);
            }

            // Подготовка данных для оригинальной таблицы
            var originalItems = variables.OrderBy(v => v).Select(v => new
            {
                Variable = v,
                Values = traceData.Select(t => t.Data.ContainsKey(v) ? t.Data[v] : "-").ToArray()
            }).ToList();

            // Подготовка данных для заполненной таблицы
            var filledItems = variables.OrderBy(v => v).Select(v => new
            {
                Variable = v,
                Values = traceData.Select(t => t.Data.ContainsKey(v) ? t.Data[v] : "-").ToArray(),
                FilledValues = FillMissingValues(v).ToArray()
            }).ToList();

            originalDataGrid.ItemsSource = originalItems;
            filledDataGrid.ItemsSource = filledItems;

            originalWindow.Content = new ScrollViewer { Content = originalDataGrid };
            filledWindow.Content = new ScrollViewer { Content = filledDataGrid };

            originalWindow.Show();
            filledWindow.Show();
        }

        // Метод для заполнения пропущенных значений
        private IEnumerable<string> FillMissingValues(string variable)
        {
            string lastValue = null;
            foreach (var state in traceData)
            {
                if (state.Data.ContainsKey(variable))
                {
                    lastValue = state.Data[variable];
                }
                yield return lastValue ?? "-";
            }
        }

        // Обновление логов
        private void UpdateLog(string message)
        {
            Dispatcher.Invoke(() => LogTextBox.AppendText(message));
        }
    }
}