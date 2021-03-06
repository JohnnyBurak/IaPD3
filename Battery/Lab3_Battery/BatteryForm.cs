﻿using System;
using System.Windows.Forms;

namespace Lab3_Battery
{
    /// <inheritdoc />
    /// <summary>
    /// Класс для GUI
    /// </summary>
    public partial class BatteryForm : Form
    {
        /// <summary>
        /// Объект, описывающий батарею
        /// </summary>
        private readonly Battery _battery = new Battery();

        /// <summary>
        /// Объект, описывающий таймер
        /// </summary>
        private Timer _timer = new Timer();

        /// <inheritdoc />
        /// <summary>
        /// Конструктор класса с формой 
        /// </summary>
        public BatteryForm()
        {
            InitializeComponent();
            
            ConnectType.Text = _battery.GetConnectType();
            AvailablePower.Text = _battery.GetAvailPower();
            AvailableTime.Text = _battery.GetAvailTime();

            TimeOfDisable.Enabled = _battery.ConnectType != Battery.OnlineStatus;
            OKButton.Enabled = _battery.ConnectType != Battery.OnlineStatus;
            prbBatteryLevel.Value = (SystemInformation.PowerStatus.BatteryLifePercent * 100 >= 0 && SystemInformation.PowerStatus.BatteryLifePercent * 100 <= 100) ? (int)(SystemInformation.PowerStatus.BatteryLifePercent * 100) : 0;

            Closing += AppClose;
        }

        /// <summary>
        /// Обработчик события Tick таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerEvent(object sender, EventArgs e)
        {
            ConnectType.Text = _battery.GetConnectType();
            AvailablePower.Text = _battery.GetAvailPower();
            AvailableTime.Text = _battery.GetAvailTime();

            TimeOfDisable.Enabled = _battery.ConnectType != Battery.OnlineStatus;
            OKButton.Enabled = _battery.ConnectType != Battery.OnlineStatus;

            if (_battery.ConnectType == Battery.OnlineStatus) _battery.ReturnOldTimeValue();
        }

        /// <summary>
        /// Обработчик события Closing закрытия формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppClose(object sender, EventArgs e)
        {
            _timer.Stop();
            _battery.ReturnOldTimeValue();
        }

        /// <summary>
        /// Обработчик нажатия на OKButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e) => _battery.DisableScreen(Convert.ToInt32(TimeOfDisable.Text));

        private void BatteryForm_Load(object sender, EventArgs e)
        {
            _timer.Interval = 100;
            _timer.Tick += TimerEvent;
            _timer.Start();

        }
    }
}
