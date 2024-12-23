import 'package:flutter/material.dart';

class TextTopWidget extends StatelessWidget {
  const TextTopWidget({super.key});

  @override
  Widget build(BuildContext context) {
    return Expanded(
      flex: 1,
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Container(
          alignment: Alignment.centerLeft,
          child: const Text.rich(
              textAlign: TextAlign.start,
              TextSpan(
                  text: "Registre-se",
                  style: TextStyle(
                    fontSize: 30,
                  ),
                  children: [TextSpan(text: "\nCrie seu usuário")])),
        ),
      ),
    );
  }
}
