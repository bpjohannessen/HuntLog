﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HuntLog.Models;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace HuntLog.Services
{
    public interface IHuntService 
    {
        Task<IEnumerable<Hunt>> GetHunts();
        Task<Hunt> GetHunt(string id);
        Task Save(Hunt hunt);
    }


    public class HuntService : IHuntService
    {

        private const int _delay = 2000;
        private List<Hunt> _hunts;
        public async Task<Hunt> GetHunt(string id)
        {
            var hunts = await GetHunts();
            return hunts.SingleOrDefault(x => x.ID == id);
        }

        public async Task<IEnumerable<Hunt>> GetHunts()
        {
            if (_hunts == null) 
            {
                await Task.Delay(_delay);

                //using (var stream = await FileSystem.OpenAppPackageFileAsync("data/jakt.xml"))
                //{
                //    using (var reader = new StreamReader(stream))
                //    {
                //        //var fileContents = await reader.ReadToEndAsync();
                //        //if (!string.IsNullOrWhiteSpace(fileContents)) 
                //        //{
                //            XmlSerializer serializer = new XmlSerializer(typeof(List<Hunt>));
                //            _hunts = (List<Hunt>) serializer.Deserialize(reader);
                //            //_hunts = JsonConvert.DeserializeObject<List<Hunt>>(fileContents);
                //        //}
                //    }
                //}

                _hunts = new List<Hunt>
                {
                    new Hunt { ID = "1", Sted = "Jonsvatnet", DatoFra = DateTime.Now },
                    new Hunt { ID = "2",  Sted = "Bymarka", DatoFra = DateTime.Now.AddDays(-10) },
                    new Hunt { ID = "3",  Sted = "Levanger", DatoFra = DateTime.Now.AddMonths(-3) }
                };
            }
            return _hunts.OrderByDescending(x => x.DatoFra);
        }

        public async Task Save(Hunt hunt)
        {
            var itemToReplace = _hunts.SingleOrDefault(x => x.ID == hunt.ID);
            if(itemToReplace != null) 
            {
                _hunts.Remove(itemToReplace);
            }
            _hunts.Add(hunt);

            await Task.CompletedTask;
        }
    }
}
