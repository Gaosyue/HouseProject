﻿using House.IRepository.DeviceManagement;
using House.IRepository.Dict;
using House.Model;
using House.Model.SystemSettings;
using House.Repository.DeviceManagement;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace House.API.Controllers.Dict
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Dict")]
    public class DictController : ControllerBase
    {
        private readonly IDictTypeRepository _IDictTypeRepository;
        private readonly IDictItemRepository _IDictItemRepository;

        public DictController(IDictTypeRepository idicttyperepository, IDictItemRepository idictitemRepository)
        {
            _IDictTypeRepository = idicttyperepository;
            _IDictItemRepository = idictitemRepository;
        }

        /// <summary>
        /// 获取全部字典类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictType>> GetDictTypeList()
        {
            //添加条件 (State状态为true)
            var predicate = PredicateBuilder.New<DictType>(true);
            
            predicate = predicate.And(t => t.State);
            //根据条件进行查询
            var data = await _IDictTypeRepository.GetAllListAsync(predicate);
            
            return data;
        } 

        /// <summary>
        /// 添加字典类
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddDictType(DictType dict)
        {
            try
            {
                return await _IDictTypeRepository.InsertAsync(dict);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        /// <summary>
        /// 根据字典类获取字典项(排序)
        /// </summary>
        /// <param name="cod"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<DictItem>> GetDictItemList(int cod)
        {
            //添加条件 (State状态为true)
            var predicate = PredicateBuilder.New<DictItem>(true);

            predicate = predicate.And(t => t.Coding==cod);
            //根据条件进行查询
            var data = await _IDictItemRepository.GetAllListAsync(predicate);


            return data.OrderBy(t=>t.OrderId).ToList();
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddDictItem(DictItem dict)
        {
            try
            {
                return await _IDictItemRepository.InsertAsync(dict);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
