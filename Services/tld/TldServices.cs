using AutoMapper;
using System;
using System.Linq;
using System.Collections.Generic;
using ProjetoRedehost.Data;
using ProjetoRedehost.Models;
using ProjetoRedehost.Exceptions;
using ProjetoRedehost.ViewModels;
using ProjetoRedehost.Services.tld.cache;

namespace ProjetoRedehost.Services.tld
{
    public class TldServices : ITld
    {
        
        private readonly ApplicationDbContext _dbContext;
        
        private readonly ITldCache _cacheService;

        private bool _saveCache = true;
        

        public TldServices(ApplicationDbContext dbContext,ITldCache cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
            #if DEBUG
            _saveCache = false;
            #endif
        }
        
        private bool existeTld(Tld value)
        {
            var tld = _dbContext.Tlds.Where(b => b.Extension == value.Extension).FirstOrDefault();    
            if (tld != null)
            {
                return true;
            }   
            return false;
        }


        public IEnumerable<Tld> ListAll()
        {
            return _dbContext.Tlds.AsEnumerable();
        }

        public Tld Find(int id)
        {
            var tld = _dbContext.Tlds.Find(id);
            if (tld == null)
            {
                throw new NotFoundException();
            }   
            return tld;
        }

        public Tld Add(Tld tld)
        {
            _dbContext.Tlds.Add(tld);
            _dbContext.SaveChanges();
            if(_saveCache) 
            {
                _cacheService.Add(tld.Extension);
            }
            return tld;
        }

        public void Edit(Tld tld)
        {
            if (existeTld(tld))
            {
                throw new BadRequestException("Tld já cadastrado.");
            }             
              
            var result = _dbContext.Tlds.Find(tld.Id);
            var previousExtension = result.Extension;
            if (result != null) 
            {
                try
                {
                    result.Extension = tld.Extension;
                    result.UsuarioAlteracao = tld.UsuarioAlteracao;
                    result.DataAlteracao = tld.DataAlteracao;
                    _dbContext.SaveChanges();
                    if(_saveCache) 
                    {
                        _cacheService.Edit(previousExtension,tld.Extension);
                    }
                }
                catch (Exception ex)
                {
                    throw new BadRequestException(ex.Message);
                }
            }
            else{
               throw new NotFoundException();
            }
        }

        public void Delete(int id)
        {
            var result = _dbContext.Tlds.Find(id);
            if (result != null)
            {
                try
                {
                    _dbContext.Remove(result);
                    _dbContext.SaveChanges();

                    if(_saveCache) 
                    {
                        _cacheService.Remove(result.Extension);
                    }
                }
                catch (Exception ex)
                {
                    throw new BadRequestException(ex.Message);
                } 
            }
            else{
               throw new NotFoundException();
            }
        }
    
    }
}