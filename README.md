No jogo, o jogador controla um tanque que combate um tanque adversário. O objectivo do jogo é operar o tanque
de modo destruir o tanque adversário.
Os tanques deslocam-se sobre um campo de batalha com terreno acidentado. A topologia do terreno respeita um
mapa de alturas fornecido. O jogo a desenvolver deverá apresentar graficamente, pelo menos, o terreno e os
tanques. A definição da geometria do terreno deverá minimizar o trafego CPU-GPU e deverá minimizar a ocupação
de memória do GPU. Deverá ser aplicada uma textura ao terreno e o jogo deverá usar iluminação. A posição e
orientação da câmara deverá ser controlada pelo utilizador e o jogo deverá incluir pelo menos 3 câmaras: uma
câmara com surface follow (câmara 1), uma câmara livre (câmara 2), e uma câmara do tipo third-person em relação
ao tanque controlado pelo jogador (câmara 3, default). Os tanques deverão exibir um comportamento que seja
coerente com a topologia do terreno, em termos de posição e orientação, através de interpolação bilinear. As
interpolações bilineares deverão ser explicitamente implementadas no código. O tanque adversário deverá, por
escolha do utilizador, poder ser controlado por um segundo jogador e ter um comportamento autónomo de
perseguição ou deambulação. O jogo deverá detectar colisões entre os tanques e actuar em conformidade. O jogo
deverá também detectar a colisão dos tanques com os limites do campo de batalha e evitar que os tanques o
ultrapassem. O movimento dos tanques deverá gerar um efeito visual semelhante à criação de pó, que deverá ser
controlado por um sistema de partículas. Pelo menos o tanque controlado pelo primeiro jogador deverá poder
disparar balas de canhão (sem disparo continuo) que destruam o tanque adversário. As balas de canhão podem ter
uma forma aproximadamente esférica, portanto podem-se utilizar esferas para o render e para a determinação de
colisões entre balas e tanques. Todas as detecções de colisões têm de ser implementadas explicitamente. É
estritamente proibido usar as classes e funções do MonoGame para implementação de bounding volumes e
detecção de colisões.
Os alunos deverão implementar o jogo em MonoGame com C#, de modo a que as regras do jogo sejam respeitadas.
Os alunos têm liberdade para usar os modelos e as texturas que entenderem para os diferentes elementos do jogo.
Os modelos 3D dos tanques (e outros elementos julgados necessários) poderão ser desenvolvidos pelos alunos ou
poderão ser obtidos em repositórios na Internet

OBS: os arquivos estao incompletos por conta do tamanho, estão apenas os Codigos
