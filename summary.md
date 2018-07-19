# Summary

郑子涵（v-zihzhe）

**花费时间**：大约3小时

**算法**：`Match`函数首先预处理pattern，把转义字符还原，把`+`替换成`?*`，两个通配符`*`和`?`用`'star'`和`'question'`表示，保存在一个字符串数组里。然后使用动态规划计算是否匹配，用一个二维数组`match[i,j]`保存pattern的前i个字符和value的前j个字符是否匹配。递推公式是：如果pattern最后一个字符不是`*`，那么`pattern[0..p] matches string[0..s] <-> pattern[0..p-1] matches string[0..s-1] && pattern[p] matches string[s]`；如果pattern最后一个字符是`*`，那么`pattern[0..p] matches string[0..s] <-> pattern[0..p-1] matches string[0..s] || pattern[0..p] matches string[0..s-1]`。

**单元测试**：我对空串、每个通配符、混合情况、转义字符都进行了测试（包括很多边界情况），转义字符如果非法（`\`后面接的不是通配符或者`\`位于字符串末尾）会抛出异常。除此之外，还加了一个随机测试，生成大量随机的串，把我的算法的结果和正则表达式的结果进行对比。测试的覆盖率是99.18%，没覆盖的部分是单元测试代码中抛出异常之后的大括号。

