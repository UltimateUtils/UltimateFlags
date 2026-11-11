# Ultimate Flags

[GitHub Repo](https://github.com/UltimateUtils/UltimateFlags/tree/main/UltimateFlags/UltimateFlags)

[Nuget](https://www.nuget.org/packages/UltimateFlags)

이것은 UltimateFlags 패키지들의 base 구현 패키지입니다.
클라이언트 코드에서 직접 참조하게 하려는 목적으로 만들어진 패키지가 아닙니다.

용도에 맞게 다음 패키지들을 사용해주세요.

## UltimateFlags.Configuration

만약 flag들의 양이 비교적 적고 on/off를 자주 해야 하지 않는다면 Configuration을 이용하여 feature flag의 목적을 달성할 수 있습니다.
매우 빠르고 효과적인 방법이지만 flag을 켜고 끄는 작업이 잦다면 DB를 이용하는 다른 패키지들을 고려해보세요.

## UltimateFlags.EF

EntityFramework Core를 이용하여 flag들을 관리하려면 이 패키지를 사용하세요.
EF Core가 지원하는 그 어떤 DB라도 사용할 수 있습니다.
하지만 MongoDB 등 Document DB를 지원하는 별도의 패키지들을 배포할 계획이 있습니다.
