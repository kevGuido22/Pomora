﻿using Pomora.Pages; 

namespace Pomora
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CronometerPage();
        }
    }
}
