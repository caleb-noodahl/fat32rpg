using MerchantRPG.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MerchantRPG
{
    public class App
    {
        private IGameEngineService _engine; 

        public App(IGameEngineService engine)
        {
            _engine = engine; 
        }
        public async Task Run()
        {
            await _engine.GameLoop(); 
        }
    }
}
