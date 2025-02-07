﻿using System;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPrinterControl
{
				public partial class MainForm : Form
				{
								private GPrinterHttp.Printer printer = new GPrinterHttp.Printer();
								private Service service = new Service();

								public MainForm()
								{
												InitializeComponent();
												btnServiceStart.Enabled = true;
												btnServiceStop.Enabled = false;
												StatusChangeHandle();
								}

								private void StatusChangeHandle()
								{
												if (service.IsServiceExisted())
												{
																ServiceControllerStatus status = service.GetServiceStatus();
																if (status == ServiceControllerStatus.Running)
																{
																				pictureBox1.Image = Properties.Resources.icon_success;
																				btnServiceStart.Enabled = false;
																				btnServiceStop.Enabled = true;
																}
																else if (status == ServiceControllerStatus.Stopped)
																{
																				pictureBox1.Image = Properties.Resources.icon_warn;
																				btnServiceStart.Enabled = true;
																				btnServiceStop.Enabled = false;
																}
																else
																{
																				pictureBox1.Image = Properties.Resources.icon_info;
																}
																btnServiceUninstall.Enabled = true;
												}
												else
												{
																btnServiceUninstall.Enabled = false;
												}
								}

								private async void btnServiceStart_Click(object sender, EventArgs e)
								{
												btnServiceStart.Enabled = false;
												btnServiceStop.Enabled = true;
												await Task.Run(() => { service.StartService(); });
												Thread.Sleep(1000);
												StatusChangeHandle();
								}

								private async void btnServiceStop_Click(object sender, EventArgs e)
								{
												btnServiceStart.Enabled = true;
												btnServiceStop.Enabled = false;
												await Task.Run(() => { service.StopService(); });
												Thread.Sleep(1000);
												StatusChangeHandle();
								}

								private async void btnServiceUninstall_Click(object sender, EventArgs e)
								{
												await Task.Run(() => { service.UninstallService(); });
												Thread.Sleep(1000);
												StatusChangeHandle();
								}

								private void btnTestUSB_Click(object sender, EventArgs e)
								{
												//try
												//{
												//				int nPortCount = GPrinterHttp.Sbarco.PortEnumCount(0);
												//				if(nPortCount > 0)
												//				{
												//								string acPortBuffer = "";
												//								GPrinterHttp.Sbarco.PortEnumGet(0, 0, acPortBuffer);
												//								if (!GPrinterHttp.Sbarco.PortOpen(acPortBuffer))
												//								{
												//												MessageBox.Show("未检测到打印机", "连接打印机");
												//								} else
												//								{
												//												GPrinterHttp.Sbarco.SetMeasurement(1, 0); // inchs
												//												GPrinterHttp.Sbarco.PrintText(1.1f, 0.2f, 0, 1, 1, 1, false, 14, "Print from USB");
												//												GPrinterHttp.Sbarco.PrintLabel(1, 1, false);
												//												GPrinterHttp.Sbarco.PortClose();
												//								}
												//				}
												//				else
												//				{
												//								MessageBox.Show("未检测到打印机", "连接打印机");
												//				}																
												//}
												//catch (Exception ex)
												//{
												//				MessageBox.Show(ex.Message, "打印机状态");
												//}
												if (printer.CheckPrinter())
												{
																printer.StartPrint("DEMO");
												}
												else
												{
																MessageBox.Show("未检测到打印机", "连接打印机");
												}
								}

								private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
								{
												if (e.Button == MouseButtons.Left)
												{
																Visible = true;
																WindowState = FormWindowState.Normal;
																Activate();
												}
								}

								private void Form1_FormClosing(object sender, FormClosingEventArgs e)
								{
												if (e.CloseReason == CloseReason.UserClosing)
												{
																e.Cancel = true;
																notifyIcon1.ShowBalloonTip(3000, "GP条码打印服务", "程序已最小化到系统托盘运行", ToolTipIcon.Info);
																Hide();
																notifyIcon1.Visible = true;
												}
								}

								private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
								{
												DialogResult result = MessageBox.Show("是否退出", "提示", MessageBoxButtons.OKCancel);
												if (result == DialogResult.OK)
												{
																Application.Exit();
												}
								}

								private void PrinterStatusToolStripMenuItem_Click(object sender, EventArgs e)
								{
												if (printer.CheckPrinter())
												{
																string status = printer.ReadDataFmUSB();
																MessageBox.Show(status, "打印机状态");
												}
								}

								private void btnSetPageHome_Click(object sender, EventArgs e)
								{
												if (printer.CheckPrinter())
												{
																printer.ResetPrinter();
												}
												else
												{
																MessageBox.Show("未检测到打印机", "连接打印机");
												}
								}
				}
}
