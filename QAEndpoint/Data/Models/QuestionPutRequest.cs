using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAEndpoint.Data.Models {
    public class QuestionPutRequest {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}

/**
 * model的字段不和数据库中表的字段完全一致的原因：
 * 1、从API的易用性去考虑：客户端每次访问仅提供能完成其业务的最小的信息即可
 * 2、从安全的角度去考虑：API接口是对所有人开放的 所有人都可以浏览接口的内容
 * => 如果API的请求与响应模型，与数据库中的字段完全一致 存在暴露应用程序
 * 内部逻辑的风险
 * * 
 */ 