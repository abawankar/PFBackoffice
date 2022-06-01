using DAL.Master;
using Domain.Implementation;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.Master
{
    public class CountryModel: Domain.Implementation.Country
    {
    }

    public class CountryRepository : RepositoryModel<CountryModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CountryModel obj)
        {
            CountryDAL dal = new CountryDAL();
            ICountry bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.Nationality = obj.Nationality;
            dal.InsertOrUpdate(bl);
        }

        public override IList<CountryModel> GetAll()
        {
            CountryDAL dal = new CountryDAL();
            AutoMapper.Mapper.CreateMap<Country, CountryModel>();
            List<CountryModel> model = AutoMapper.Mapper.Map<List<CountryModel>>(dal.GetAll());

            return model;
        }

        public override CountryModel GetById(int id)
        {
            CountryDAL dal = new CountryDAL();
            AutoMapper.Mapper.CreateMap<Country, CountryModel>();
            CountryModel model = AutoMapper.Mapper.Map<CountryModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CountryModel obj)
        {
            CountryDAL dal = new CountryDAL();
            ICountry bl = new Country();
            bl.Name = obj.Name;
            bl.Nationality = obj.Nationality;
            dal.InsertOrUpdate(bl);
        }
    }
}