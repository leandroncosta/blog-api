import 'package:flutter/material.dart';

class InputWidget extends StatelessWidget {
  final String label;
  final TextEditingController controller;

  const InputWidget({super.key, required this.label, required this.controller});

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;

    return SizedBox(
      width: size.width * 0.90,
      child: TextField(
        controller: controller,
        decoration: InputDecoration(
            fillColor: Colors.grey.withOpacity(0.2),
            filled: true,
            labelText: label,
            focusedBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(12),
              borderSide: const BorderSide(color: Colors.yellow),
            ),
            border: OutlineInputBorder(
                borderSide: BorderSide.none,
                borderRadius: BorderRadius.circular(15))),
      ),
    );
  }
}
