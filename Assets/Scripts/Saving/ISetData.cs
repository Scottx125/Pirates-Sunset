using System;

public interface ISetData
{
    public void SetData<T, TData>(T key, object value) where T : Enum where TData : struct;
}
