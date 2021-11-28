using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Services
{
    public interface IListMappingService
    {
        void ChangeRelationship(
            SecretSantaModel owner,
            SecretSantaModel entityToBeChanged,
            SecredSantaMappingType destinationType
        );

        SecredSantaMappingType GetRelationshipType(
            SecretSantaModel owner,
            SecretSantaModel entityToBeChanged
        );

        int GetCount(SecretSantaModel secretSantaModel, SecredSantaMappingType type);

        List<SecretSantaModel> GetHoleList(
            SecretSantaModel santaModel,
            SecredSantaMappingType type,
            bool asNoTracking = false
        );

        void RemoveSanta(SecretSantaModel model);

        int CountOfAppearOnList(SecretSantaModel model, SecredSantaMappingType list);
    }

    public class ListMappingService : IListMappingService
    {
        private readonly ConfigContext _dbContext;

        public ListMappingService()
        {
            _dbContext = new ConfigContext();
        }

        public void ChangeRelationship(
            SecretSantaModel owner,
            SecretSantaModel entityToBeChanged,
            SecredSantaMappingType destinationType
        )
        {
            var existEntity = GetEntity(owner, entityToBeChanged, false)
                              ?? new ListMapping { Id = new Guid() };

            existEntity.MappingType = destinationType;
            _dbContext.Update(existEntity); //evtl. muss ein Add genutzt werden...
        }

        public SecredSantaMappingType GetRelationshipType(
            SecretSantaModel owner,
            SecretSantaModel entityToBeChanged
        )
        {
            var entity = GetEntity(owner, entityToBeChanged, false)
                         ?? new ListMapping { Id = new Guid() };

            _dbContext.Update(entity); //evtl. muss ein Add genutzt werden...
            return entity.MappingType;
        }

        private ListMapping GetEntity(
            SecretSantaModel owner,
            SecretSantaModel entityToBeChanged,
            bool asNoTracking
        )
        {
            if (asNoTracking)
            {
                return _dbContext.ListMappings.AsNoTracking()
                    .FirstOrDefault(
                        entity => entity.Owner == owner
                                  && entity.SecretSanta == entityToBeChanged
                    );
            }

            return _dbContext.ListMappings
                .FirstOrDefault(
                    entity => entity.Owner == owner
                              && entity.SecretSanta == entityToBeChanged
                );
        }

        public int GetCount(SecretSantaModel secretSantaModel, SecredSantaMappingType type)
        {
            var allEntries = _dbContext.ListMappings.Where(
                    p => p.Owner == secretSantaModel
                         && p.MappingType == type
                )
                .AsNoTracking();

            return allEntries.Count();
        }

        public List<SecretSantaModel> GetHoleList(
            SecretSantaModel santaModel,
            SecredSantaMappingType type,
            bool asNoTracking = false
        )
        {
            IIncludableQueryable<ListMapping, SecretSantaModel> listMappings;
            if (asNoTracking)
            {
                listMappings = _dbContext.ListMappings
                    .Where(p => p.Owner == santaModel && p.MappingType == type)
                    .AsNoTracking()
                    .Include(p => p.SecretSanta);
            }
            else
            { 
                listMappings = _dbContext.ListMappings
                    .Where(p => p.Owner == santaModel && p.MappingType == type)
                    .Include(p => p.SecretSanta);
            }

            return listMappings.Select(oneMapping => oneMapping.SecretSanta).ToList();
        }

        public void RemoveSanta(SecretSantaModel model)
        {
            var entities =
                _dbContext.ListMappings.Where(p => p.Owner == model || p.SecretSanta == model);

            _dbContext.RemoveRange(entities);
        }


        public int CountOfAppearOnList(SecretSantaModel model, SecredSantaMappingType list)
        {
            return _dbContext.ListMappings
                .Where(p => p.SecretSanta == model && p.MappingType == list)
                .AsNoTracking()
                .Count();
        }
    }
}