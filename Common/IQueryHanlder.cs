namespace Common
{
    public interface IQueryHanlder<Req, Res>
        where Req : class
        where Res : class
    {
        Res ExecuteQuery(Req req);
    }
}
