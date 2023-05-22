import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Exercicio3 {
    private static class Aluno {
        public int codigo;
        public List<Float> notas = new ArrayList<>();
    }

    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        int n = 5;
        List<Aluno> alunos = new ArrayList<>();
        for (int i = 0; i < n; i++) {
            Aluno a = new Aluno();
            a.codigo = scan.nextInt();
            a.notas.add(scan.nextFloat());
            a.notas.add(scan.nextFloat());
            a.notas.add(scan.nextFloat());
            alunos.add(a);
        }

        alunos.forEach(a -> {
            float maior = a.notas.stream().max(Float::compare).get();
            System.out.printf("Codigo: %d\n", a.codigo);
            System.out.printf("Notas: %f %f %f\n", a.notas.get(0), a.notas.get(1), a.notas.get(2));
            float media = (a.notas.stream().filter(x -> x!=maior).map(x -> x*3).reduce(0.0f, Float::sum)
                    + maior*4)/10;
            System.out.printf("Media final: %f\n", media);
            if(media >= 7){
                System.out.println("APROVADO");
            }else{
                System.out.println("REPROVADO");
            }
        });
    }
}
