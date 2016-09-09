using System;
using System.Collections.Generic;
using Tagolog.NLog.Model;

namespace Tagolog.NLog.Repository
{
    internal interface ITagRepository
    {
        List<TagEntity> Save( Guid customerId, List<string> tagCodes );

        //Guid Insert( TagToInsert item );
        //void Update( Tag item );

        ///// <summary>
        ///// Try to get tag by code. If it isn't exist - create it
        ///// </summary>
        ///// <param name="customerId"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //Tag GetOrInsert( Guid customerId, string code );

        //Tag GetById( Guid tagId );
        //Tag GetByCode( Guid customerId, string code );

        //IList<Tag> GetTagList();
        //IList<Tag> GetTagListForCustomer( Guid customerId );
    }
}
