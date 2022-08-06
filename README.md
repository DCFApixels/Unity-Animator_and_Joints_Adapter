# Unity-Animator_and_Joints_Adapter
A script for combining the work of animations and joints.

Небольшой скрипт решающий проблему сочетания анимаций и Joint-ов. Проект содержит демо-сцену и основной скрипт CopyTransform.cs (В свой проект достаточно скопировать только этот скрипт)

![image](https://user-images.githubusercontent.com/99481254/183257657-909410db-04cb-4bab-ae1e-3213ea4575d6.png)

Принцип работы заключается в том, чтобы для анимаций создать копию скелета с Animator, а с основного скелета для каждой кости повторять состояние Transform с аналогичных костей из копии, далее в основном скелете настроить Configurable Joint-ы. Скрипт CopyTransform.cs отвечает за такое повторение. Если вместе с CopyTransform на обекте висит Configurable Joint то он будет идентично повторять localPosition и localScale, а поворот будет происходит через joint, если нет Configurable Joint, то будет повторять и localRotation.

Для удобства скрипт содержит в себе генератор. Что бы сгенерировать копию, выберете нужный объект в иерархии(можно несколько), после чего кликните по "GameObject/Generate Animator-Joins" или через ПКМ кликните по "Generate Animator-Joins". Алгоритм найдет подходящий GameObject содержащий Animator и сделает его копию с привязками, а Animator перенесет на копию. 
![image](https://user-images.githubusercontent.com/99481254/183141213-cfe5f285-d1af-4455-9d16-21e0bbe79d27.png)
![image](https://user-images.githubusercontent.com/99481254/183141297-c7a03d7a-6ab7-4770-b0ce-5918622cf2f7.png)

Если выбранный GameObject не содержит Animator, то алгоритм сначала попытается его найти в цепочке родителей, если не найдет, то попытается найти среди потомков, если среди потомков будет только один Animator то генерация копии будет для этого GameObject. В противном случае выдаст ошибку.

Скрипт CopyTransform работает как в режиме воспроизведения, так и в режиме редактирования, но в режиме редактирования не работают Joint-ы, поэтому анимации можно создавать в режиме редактирования, основной скелет будет повторять движения скелета с аниматором.
