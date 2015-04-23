namespace IBLL
{
    /// <summary>
    /// 数据仓储工厂
    /// </summary>
    public interface IDbSessionFactory
    {
        IBLLSession GetBLLSession();
    }
}
