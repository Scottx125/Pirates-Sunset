using System;
public interface IGetData 
{
    public TData GetData<T, TData>(T key) where T : Enum where TData : struct;
}
