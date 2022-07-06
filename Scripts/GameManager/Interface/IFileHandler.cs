/**
 * 인터페이스 분리가 무슨 의미가 있는 건지 모르겠다.
 * 만들어 두면 좋긴 할까?
 * GameManager가 Monolithic해지는 것을 막아야 하는 건가?
 * 함께 고민해볼 문제입니다...
 */

public interface IFileHandler : IDataHandler
{
    public void Save();
    public void Load();
}
