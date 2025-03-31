using System;

using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexDisk.Client;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using WForms_App_Filemanager.Properties;
using System.Linq;
using System.IO;


namespace WForms_App_Filemanager
{
    public partial class Form1 : Form
    {
        
        private TreeView _treeView;
        private ListView _listView;
        private Button _refreshButton;
        private Button _createFolderButton;
        //private readonly YandexDiskClient _httpClient;
        private string _path = "/";
        private readonly DiskHttpApi clientApi;
        private readonly string _token = "y0__xDrtJkFGLijNiCk6JPNEvRjUsH5le-oyOMziushmDklJQl0";

        public Form1()
        {
            InitializeComponent();
            SetupUI();
            clientApi = new DiskHttpApi(_token);
            //_httpClient = new YandexDiskClient(_token);
    }

        private void SetupUI()
        {
            _treeView = new TreeView { Dock = DockStyle.Left, Width = 200 };
            _listView = new ListView { Dock = DockStyle.Fill, View = View.Details };
            _listView.Columns.Add("Имя", 200);
            _listView.Columns.Add("Тип", 100);
            _listView.Columns.Add("Путь", 100);

            _refreshButton = new Button { Text = "Обновить", Dock = DockStyle.Top };
            _refreshButton.Click += RefreshButton_Click;
            _createFolderButton  = new Button { Text = "Перейти в папку", Dock = DockStyle.Left };
            _createFolderButton.Click += FolderButton_Click;

            _listView.DoubleClick += _listView_DoubleClick;

            Controls.Add(_listView);
            Controls.Add(_treeView);
            Controls.Add(_refreshButton);
            Controls.Add(_createFolderButton);
        }

        private void _listView_DoubleClick(object sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) return;
            ListViewItem selectedItem = _listView.SelectedItems[0];
            string name = selectedItem.Name.ToString();
        }

        private async void FolderButton_Click(object sender, EventArgs e)
        {
            var folder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = _path
            });

            var files = folder.Embedded.Items;
            foreach (var file in files)
            {
                if (string.Equals(file.Type.ToString(), "Dir") && _listView.CheckedItems.Count == 0)
                {
                    try
                    {
                        _path += file.Name.ToString();

                        folder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
                        {
                            Path = _path,
                        });
                        
                        _path = $"{folder.Path}/";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка, выберите папку", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            await LoadFilesAsync(folder);
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            var folder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
            {
                Path = "/"
            });
            await LoadFilesAsync(folder);
        }

        protected async Task LoadFilesAsync(Resource resource)
        {           

            var files = resource.Embedded.Items;
            _listView.Items.Clear();

            foreach (var item in files)
            {
                
                    var listItem = new ListViewItem(item.Name.ToString());
                    listItem.SubItems.Add(item.Type.ToString());
                    listItem.SubItems.Add(item.Path);
                    _listView.Items.Add(listItem);
                
            }
        }
    }
}
