///   --------------------------------------------------------------------------------------------------------------------\\\
///                           C# Tutorial - Import data from Excel to SQL Server _ FoxLearn.mp4                            \\\
///   ----------------------------------------------------------------------------------------------------------------------\\\


using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;

namespace foxLearnExcel
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();

            // تسجيل مزود الترميز
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        DataTableCollection tableCollection;
        private DataTable currentData; // لتخزين البيانات الحالية

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // فتح مسار الملف وتخزين الاسم في مربع النص
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select an Excel File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;

                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true // استخدام الصف الأول كعناوين
                                    }
                                });

                                tableCollection = result.Tables;
                                cmbSheetsName.Items.Clear();

                                foreach (DataTable table in tableCollection)
                                {
                                    cmbSheetsName.Items.Add(table.TableName); // إضافة أسماء الجداول إلى القائمة المنسدلة
                                }

                                // تحديد الصفحة الأولى تلقائيًا
                                if (cmbSheetsName.Items.Count > 0)
                                {
                                    cmbSheetsName.SelectedIndex = 0; // اختيار الصفحة الأولى
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"حدث خطأ أثناء قراءة الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                ValidateNames = true,
                Multiselect = false
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;

                    try { using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });
                            tableCollection = result.Tables;
                            cmbSheetsName.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cmbSheetsName.Items.Add(table.TableName);

                                // تحديد الصفحة الأولى تلقائيًا
                                if (cmbSheetsName.Items.Count > 0)
                                {
                                    cmbSheetsName.SelectedIndex = 0; // اختيار الصفحة الأولى
                                }
                            }
                    }
                    
                    }
                    
                    catch(Exception ex) { MessageBox.Show($"حدث خطأ أثناء قراءة الملف: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }


                    
                }
            };

        }
        private void cmbSheetsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSheetsName.SelectedItem != null)
            {
                string selectedSheet = cmbSheetsName.SelectedItem.ToString();
                DataTable dt = tableCollection[selectedSheet];
                currentData = dt; // تخزين البيانات الحالية
                dataGridView1.DataSource = dt; // عرض البيانات في DataGridView
            }
        }



        private void btnSort_Click(object sender, EventArgs e)
        {
            if (currentData != null)
            {
                // فرز البيانات بناءً على العمود الأول (يمكن تعديله لاختيار عمود آخر)
                var sortedData = currentData.AsEnumerable()
                    .OrderBy(row => row.Field<string>(0)) // فرز حسب العمود الأول
                    .CopyToDataTable();

                dataGridView1.DataSource = sortedData; // تحديث الجدول بالبيانات المرتبة
            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (currentData != null)
            {
                string searchText = textEdit1.Text.ToLower(); // الحصول على نص البحث
                var filteredData = currentData.AsEnumerable()
                    .Where(row => row.ItemArray.Any(field => field.ToString().ToLower().Contains(searchText)))
                    .CopyToDataTable();

                dataGridView1.DataSource = filteredData; // تحديث الجدول بالبيانات المرشحة
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentData != null)
            {
                string searchText = textEdit1.Text.ToLower(); // الحصول على نص البحث
                var filteredData = currentData.AsEnumerable()
                    .Where(row => row.ItemArray.Any(field => field.ToString().ToLower().Contains(searchText)))
                    .CopyToDataTable();

                dataGridView1.DataSource = filteredData; // تحديث الجدول بالبيانات المرشحة
            }
        }

    }
}


/*
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);



namespace foxLearnExcel
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            // تسجيل مزود الترميز
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }

        DataTableCollection tableCollection;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                ///-----------------فتح مسار الملف وتخزين الاسم في مربع النص ----------------------------
                openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
                openFileDialog.Title = "Select an Excel File";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;

                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {


                                //ConfigureDataTable = (_) => ExcelDataTableConfiguration()
                                //{
                                //    UserHeadRow = true
                                //}

                            });
                            tableCollection = result.Tables;
                            cmbSheetsName.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cmbSheetsName.Items.Add(table.TableName);//اضافة اسماء الجداول الى القائمة المنسدلة
                        }
                    }
                }
                ///-------------------------------------------------------------------
                ///


            }
        }


        private void cmbSheetsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[cmbSheetsName.SelectedItem.ToString()];

            dataGridView1.DataSource = dt;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Excel Files|*.xlsx;*.xls",
                ValidateNames = true,
                Multiselect = false
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                {
                                    UseHeaderRow = true
                                }
                            });
                            tableCollection = result.Tables;
                            cmbSheetsName.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cmbSheetsName.Items.Add(table.TableName);
                        }
                    }
                }
            };


        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
*/