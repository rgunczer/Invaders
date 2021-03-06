﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;


namespace InvadersDatFileConverter
{
    public partial class frmMain : Form
    {   
        private const int kDrawingXOffset = 190;

        private string _sourceDatFilesPath;
        private string _destJsonFilesPath;
        
        private Loader _loader;
        private Painter _painter;
        private Converter _converter;
    
        // ctor
        public frmMain()
        {
            InitializeComponent();
            _sourceDatFilesPath = ConfigurationManager.AppSettings["pathToDatFiles"];
            _destJsonFilesPath = ConfigurationManager.AppSettings["pathToJsonFiles"];

            _loader = new Loader(_sourceDatFilesPath);
            _painter = new Painter(kDrawingXOffset);
            _converter = new Converter(_destJsonFilesPath);            
        }

        // form events

        private void frmMain_Load(object sender, EventArgs e)
        {
            cboDrawScale.SelectedIndex = 0;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            _painter.ReDraw(CreateGraphics());
        }
            
        // zoom           
        private void cboDrawScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedScale = (string)cboDrawScale.SelectedItem;

            selectedScale = selectedScale.Replace("x", "");
            int scale = Convert.ToInt32(selectedScale);
            _painter.Scale = scale;
            _painter.ReDraw(CreateGraphics());
        }        

        // Get

        private List<byte> GetTitleData()
        {
            return _loader.LoadTitle("MOL1.DAT");
        }

        private List<byte> GetEnemyData()
        {
            return _loader.LoadEnemy("MOL.DAT");
        }

        private List<byte> GetPlayerData()
        {
            return _loader.LoadPlayer("MOL.DAT");
        }

        private List<byte> GetPlayerLiveData()
        {
            return _loader.LoadPlayerLive("MOL2.DAT");
        }

        // Draw

        private void btnLoadAndDisplayEnemy_Click(object sender, EventArgs e)
        {
            _painter.DrawEnemy(CreateGraphics(), GetEnemyData());
        }

        private void btnLoadAndDisplayPlayerLive_Click(object sender, EventArgs e)
        {            
            _painter.DrawPlayer(CreateGraphics(), GetPlayerLiveData());
        }

        private void btnLoadAndDisplayPlayer_Click(object sender, EventArgs e)
        {            
            _painter.DrawPlayer(CreateGraphics(), GetPlayerData());
        }

        private void btnLoadAndDisplayTitle_Click(object sender, EventArgs e)
        {
            _painter.DrawTitle(CreateGraphics(), GetTitleData());
        }

        // Convert

        private void btnConvertTitle_Click(object sender, EventArgs e)
        {
            const string fileName = "title.json";
            string fullPathToConvertedFile = _converter.ConvertTitle(GetTitleData(), fileName);
            toolStripStatusInfo.Text = " '" + fileName + "' successfully saved at '" + fullPathToConvertedFile + "'";
        }

        private void btnConvertPlayerLive_Click(object sender, EventArgs e)
        {
            const string fileName = "player_live.json";
            string fullPathToConvertedFile = _converter.ConvertPlayerLive(GetPlayerLiveData(), fileName);
            toolStripStatusInfo.Text = " '" + fileName + "' successfully saved at '" + fullPathToConvertedFile + "'";
        }

        private void btnConvertPlayer_Click(object sender, EventArgs e)
        {
            const string fileName = "player.json";
            string fullPathToConvertedFile = _converter.ConvertPlayer(GetPlayerData(), fileName);
            toolStripStatusInfo.Text = " '" + fileName + "' successfully saved at '" + fullPathToConvertedFile + "'";
        }

        private void btnConvertEnemy_Click(object sender, EventArgs e)
        {
            const string fileName = "enemy.json";
            string fullPathToConvertedFile = _converter.ConvertEnemy(GetEnemyData(), fileName);
            toolStripStatusInfo.Text = " '" + fileName + "' successfully saved at '" + fullPathToConvertedFile + "'";
        }

       
    }
}
