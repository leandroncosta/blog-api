import 'package:flutter/material.dart';

class ButtonRegisterWidget extends StatelessWidget {
  final VoidCallback onTap;
  final String label;

  const ButtonRegisterWidget(
      {super.key, required this.label, required this.onTap});

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;

    return Container(
        color: Colors.black,
        width: size.width * 0.90,
        child: TextButton(
            onPressed: onTap,
            child: Text(
              label,
              style: const TextStyle(color: Colors.white, fontSize: 18),
            )));
  }
}
