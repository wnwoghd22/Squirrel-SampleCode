/** state machine을 쓰는 이유
 * 각 상태에서만 필요가 있는 변수는 상태 안에 넣어버리고
 * 업데이트 함수를 잘게 쪼개서 삭 상태에서 필요한 부분만 업데이트해주고
 * 어떤 상태에서 버그가 일어나는지 확인이 쉬워지고, 
 * HandleInput과 Update 메소드 크기가 작아지는 듯한 효과가 있습니다.
 */

public interface IState
{
    IState HandleUpdate();
    IState HandleInput();
}
