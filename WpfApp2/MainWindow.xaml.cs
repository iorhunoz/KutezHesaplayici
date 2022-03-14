using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
 
 
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp2
{
 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int braceletSize = 0;
 
        public string serverResponse = "";
        public string webApi = "https://kutez.com/testapi/get_diameter.php";



        public MainWindow()
        {
           
            InitializeComponent();
        
        }
        private void log(string logx)
        {
            txtLog.AppendText(DateTime.Now.ToString()+" -> " + logx + System.Environment.NewLine);
         
        }
        private void btnSendServer_Click(object sender, RoutedEventArgs e)
        {

            Boolean success = true;
        
             /*
             * get braceletSize Value
             * number 1-2-3-4-5 required
             */
            try
            {

                braceletSize = Int32.Parse(txtBraceletSize.Text);

                if (braceletSize > 5 || braceletSize < 1)
                {
                    string msg_ = "Please enter numbers from 1 to 5 ";
                    MessageBox.Show(msg_);
                    success = false;
                    log(msg_);
                }

            }
            catch(Exception ex)
            {
                log(ex.Message );
                success = false;
                
            }

            if (success == false) return;

            //SENDING SERVER AND CAPTURE DATA
            try
            {
                log("Sending...");
                using (WebClient client = new WebClient())
                {
                    serverResponse = client.DownloadString(webApi+"?size="+braceletSize.ToString());
                }
                log("Sending.OK");

            }
            catch(Exception ex)
            {
                log("Sending.ERROR:" + ex.Message);
                success = false;
            }

            if (success == false) return;

            //PARSE JSON
            try
            {
                log("Parse Data..");
                resData responseData =  JsonConvert.DeserializeObject<resData>(serverResponse);
             

                if (responseData.status.ToLower() == "success")
                {
                    log("ParseData.OK");
                    txtBigD.Text = responseData.BigDiameter.ToString();
                    txtSmallD.Text = responseData.SmallDiameter.ToString();
                }
                else
                {
                    clear();
                    log("ParseData.FAIL");
                }
                    

            }
            catch(Exception ex)
            {
                clear();
                log("ParseData.ERROR" + ex.Message);
            }

           
        }

        //CLEAR Big And Small Diameter textbox
        private void clear()
        {
            txtBigD.Text = "0";
            txtSmallD.Text = "0";
            txtResult.Text = "0";
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
           
            double Perimeter;
            double a = Convert.ToDouble(txtBigD.Text) / 2;
            double b = Convert.ToDouble(txtSmallD.Text) / 2;

            Perimeter = (double)2 * 3.14 *
                     Math.Sqrt((a * a + b * b) / (2 * 1.0));

            txtResult.Text = Math.Round(Perimeter, 2).ToString();


        }
    }
}
