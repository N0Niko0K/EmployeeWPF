using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.IO;

namespace EmployeeWPF
{
    public partial class MainWindow : Window
    {
        private string _xmlFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Employee.xml"); // Путь к XML-файлу.

        public MainWindow()
        {
            InitializeComponent();

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите ФИО.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                FullNameTextBox.Focus();
                return;
            }

            string fullName = FullNameTextBox.Text;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_xmlFilePath);

                XmlNode employeeNode = null;

                // Поиск сотрудника по ФИО.
                foreach (XmlNode node in doc.SelectNodes("//Employee"))
                {
                    if (node.SelectSingleNode("FullName")?.InnerText == fullName) // Added null check
                    {
                        employeeNode = node;
                        break;
                    }
                }

                if (employeeNode != null)
                {
                    // Заполнение полей данными из XML.
                    GenderComboBox.SelectedItem = employeeNode.SelectSingleNode("Gender")?.InnerText; // Added null check

                    // Added null check and try-parse to handle potential DateTime parsing errors
                    if (DateTime.TryParse(employeeNode.SelectSingleNode("BirthDate")?.InnerText, out DateTime birthDate))
                    {
                        BirthDatePicker.SelectedDate = birthDate;
                    }
                    else
                    {
                        BirthDatePicker.SelectedDate = null;  // Handle parsing failure, e.g., set to null
                    }

                    PassportTextBox.Text = employeeNode.SelectSingleNode("Passport")?.InnerText; // Added null check
                    AddressTextBox.Text = employeeNode.SelectSingleNode("Address")?.InnerText; // Added null check
                    PositionComboBox.SelectedItem = employeeNode.SelectSingleNode("Position")?.InnerText; // Added null check
                    EducationComboBox.SelectedItem = employeeNode.SelectSingleNode("Education")?.InnerText; // Added null check

                    // Added null check and try-parse to handle potential DateTime parsing errors
                    if (DateTime.TryParse(employeeNode.SelectSingleNode("HireDate")?.InnerText, out DateTime hireDate))
                    {
                        HireDatePicker.SelectedDate = hireDate;
                    }
                    else
                    {
                        HireDatePicker.SelectedDate = null; // Handle parsing failure
                    }

                    // Обработка даты увольнения (может отсутствовать).
                    string fireDateString = employeeNode.SelectSingleNode("FireDate")?.InnerText; // Added null check
                    if (string.IsNullOrEmpty(fireDateString))
                    {
                        FireDatePicker.SelectedDate = null; // Дата увольнения не указана.
                    }
                    else
                    {
                        // Added try-parse to handle potential DateTime parsing errors
                        if (DateTime.TryParse(fireDateString, out DateTime fireDate))
                        {
                            FireDatePicker.SelectedDate = fireDate;
                        }
                        else
                        {
                            FireDatePicker.SelectedDate = null; // Handle parsing failure
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Сотрудник не найден.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearForm();
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("XML-файл не найден: " + _xmlFilePath, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при чтении XML-файла: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            FullNameTextBox.Clear();
            GenderComboBox.SelectedItem = null;
            BirthDatePicker.SelectedDate = null;
            PassportTextBox.Clear();
            AddressTextBox.Clear();
            PositionComboBox.SelectedItem = null;
            EducationComboBox.SelectedItem = null;
            HireDatePicker.SelectedDate = null;
            FireDatePicker.SelectedDate = null;
        }
    }
}
